using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AtmCommon.ViewModels;

namespace AtmLoader.ViewModels
{
    public class ItemComponent : TemplateViewModel
    {
        public string Title { get; set; }
        public ITEM_TYPE Type { get; set; }
        public ItemComponentMethod Method { get; set; }
    }

    public enum ITEM_TYPE
    {
        REGEDIT =0,
        GP
    }

    public enum METHOD_TYPE
    {
        REG_WORD = 0,
        REG_SZ
    }

    public enum METHOD_STATUS
    {
        Disabled =0,
        Enabled=1,
        UnKnown=2,
        NoExist=3
    }
    public abstract class ItemComponentMethod
    {
        public abstract string CreateScript();
        public abstract bool RunScript();
        public abstract object CurrentValue { get; }
        public abstract object SetValue { get; set; }
        public abstract string description();
    }

    public class ItemComponentRegMethod : ItemComponentMethod
    {
        string _path;
        RegistryValueKind _valuetype;
        string _valuename;
        object _value;
        object _newvalue;
        METHOD_STATUS _valuestatus;
        IDictionary<METHOD_STATUS, object> _valuekind = new Dictionary <METHOD_STATUS,object>();

        public string Path { get { return _path; } set { _path = value; update(); } }
        public RegistryValueKind ValueType { get { return _valuetype; }}
        public string ValueName { get { return _valuename; } set { _valuename = value; update(); } }

        public override object CurrentValue { get { return _value; } }
        public override object SetValue { get { return _newvalue; } set { _newvalue = value; } }
        public override string description()
        {
            return $"Regpath={Path}";
        }

        public METHOD_STATUS ValueStatus { get { return _valuestatus; } }
        private object oldValue { get; set; }
        public override string CreateScript()
        {

            return "";
        }
        public override bool RunScript()
        {
            return true;
        }

        public void AddValueKind(METHOD_STATUS kind,object kindvalue)
        {
            _valuekind.Add(kind, kindvalue);
        }

        public METHOD_STATUS GetStatusFromValue(object obj,RegistryValueKind _kind)
        {
            if (obj == null)
            {
                return  METHOD_STATUS.NoExist;
            }
            else
            {
                METHOD_STATUS ret = METHOD_STATUS.UnKnown;
                foreach (KeyValuePair<METHOD_STATUS, object> vk in _valuekind)
                {
                    if (_kind == RegistryValueKind.String || _kind == RegistryValueKind.ExpandString || _kind == RegistryValueKind.MultiString)
                    {
                        if (vk.Value.ToString() == "L" && obj.ToString().Length > 0)
                        {
                            ret = vk.Key; break;
                        }
                        else if (vk.Value.ToString() == obj.ToString())
                        {
                            ret = vk.Key; break;
                        }
                    }
                    else if (_kind == RegistryValueKind.DWord || _kind == RegistryValueKind.QWord)
                    {
                        if ((int)vk.Value == (int)obj)
                        {
                            ret = vk.Key; break;
                        }
                    }
                }

                return ret;
            }
        }

        private void update()
        {
            if(_path != null && _valuename !=null) {
                _value = readFromRegedit(_path, _valuename,out _valuetype);

                _valuestatus =GetStatusFromValue(_value, _valuetype);                
            }
        }

        private void changeValue()
        {
            if(writeToRegedit(_path, _valuename, _newvalue))
            {

            }
        }

        private bool writeToRegedit(string keypath, string key, object value)
        {
            RegistryKey sk = null;
            string[] path_items = keypath.Split('\\');

            if (path_items[0] == "HKEY_CURRENT_USER")
            {
                sk = Registry.CurrentUser.CreateSubKey(string.Join("\\", path_items, 1, path_items.Length - 1));
            }
            else if (path_items[0] == "HKEY_LOCAL_MACHINE")
            {
                sk = Registry.LocalMachine.CreateSubKey(string.Join("\\", path_items, 1, path_items.Length - 1));
            }

            try
            {
                sk.SetValue(key, value);
                sk.Close();
                return true;
            }
            catch (Exception)
            {
                sk.Close();
                return false;
            }
        }

        private object readFromRegedit(string keyPath, string key,out RegistryValueKind VType)
        {
            RegistryKey sk = null;
            string[] path_items = keyPath.Split('\\');

            if (path_items[0] == "HKEY_CURRENT_USER")
            {
                sk = Registry.CurrentUser.OpenSubKey(string.Join("\\", path_items, 1, path_items.Length - 1));
            }
            else if (path_items[0] == "HKEY_LOCAL_MACHINE")
            {
                sk = Registry.LocalMachine.OpenSubKey(string.Join("\\", path_items, 1, path_items.Length - 1));
            }

            if (sk == null)
            {
                VType = RegistryValueKind.Unknown;
                return null;
            }
            else
            {
                try
                {
                    VType = sk.GetValueKind(key);
                    
                    return sk.GetValue(key);
                }
                catch (Exception)
                {
                    VType = RegistryValueKind.Unknown;
                    return null;
                }
            }
        }

    }
}
