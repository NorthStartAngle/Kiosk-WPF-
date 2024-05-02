using System;
using System.ComponentModel;
using System.Transactions;
using System.Windows;
using System.Windows.Media;

using static AtmUi.AtmApi;
using AtmCommon.ViewModels;

namespace AtmUi.ViewModels {
    public class Transaction : TemplateViewModel {
        public Transaction() {
            UserName = " ";
            TransactionID = " ";
        }

        private string? _userName;
        public string? UserName {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        private string? _trxID;
        public string? TransactionID {
            get => _trxID;
            set { _trxID = value; OnPropertyChanged(); }
        }

        private DateTime _timeStamp;

        public DateTime TimeStamp {
            get => _timeStamp;
            set { _timeStamp = value; OnPropertyChanged(); }
        }

        private string? _languageID;

        public string? LanguageID {
            get => _languageID;
            set { _languageID = value; OnPropertyChanged(); }
        }

        public void Update(AtmApi.TransactionData transactionData) {
            UserName = transactionData.userName;
            TransactionID = transactionData.trxID;
            LanguageID = transactionData.languageID;
            TimeStamp = DateTime.FromBinary(transactionData.transactionTime);

        }
    }
}
