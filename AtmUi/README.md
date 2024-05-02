# WPF GUI Prototype

## TODO for This Document

* Working manner with MVVM for this project
* Explain navigation
* Method of styles, philosophy, mechanics

## Running Debug/Test
By default, this application runs in a 1080 x 1920 portrait HD window. In order to view 
the application in a draggable, resizeable window, edit the App.config file and change 
the **IsDevTest** setting to True. This will make it easier to see the full contents of each 
page.

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <configSections>
            <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" >
                <section name="AtmUi.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
            </sectionGroup>
        </configSections>
        <applicationSettings>
            <AtmUi.Properties.Settings>
                <setting name="StartPage" serializeAs="String">
                    <value>Pages/AtmStart.xaml</value>
                </setting>
                <!-- Change this setting to True to run the application in a window -->
                <setting name="IsDevTest" serializeAs="String">
                    <value>False</value>
                </setting>
                <setting name="DefaultButtonFontSize" serializeAs="String">
                    <value>28</value>
                </setting>
                <setting name="DefaultTitleFontSize" serializeAs="String">
                    <value>36</value>
                </setting>
                <setting name="DefaultDescriptionFontSize" serializeAs="String">
                    <value>28</value>
                </setting>
                <setting name="DefaultFontFamily" serializeAs="String">
                    <value>Segoe UI Bold</value>
                </setting>
            </AtmUi.Properties.Settings>
        </applicationSettings>
    </configuration>

This project use a concept of template pages in WPF 
and use them to create basic navigation through an ATM-like application. The styling 
is still very basic, and the buttons do actually do anything except switch from one 
screen to another, but there is now enough basic scaffolding to support addition of 
business logic.

## Template Pages
The idea being demonstrated here is that there are usually only a handful of standard 
page layouts in a kiosk application, but there are several instances of these layouts 
in the actual screens shown to the user. In this example, there are only two so far: 
Template_StartPage and Template_BasePage. The Template_StartPage template has a single 
button in the middle, and Template_BasePage has four buttons down each side of the screen. 
These are then pulled into real page instances and modified as needed.

## Pre-Requisites
The project is built with .NET Core 7.0, so it will need to be installed and added to 
Visual Studio if it isn't present already. (TODO: Add instructions here)

## Caveats
Naturally, this is nowhere near being a real ATM UI. The real app would run full-screen 
in kiosk mode, have several more screen templates and instances of those templates, and 
would be styled much more appealingly. What this project shows is a proof-of-concept for 
how to approach creating screens and business logic in a WPF kiosk application.

## Screen Templates

As mentioned above, screen templates implement a basic layout that may be used on multiple 
screens in an application. Templates are defined as XAML user controls with C# code added 
in support. They are not used directly but are instead included on WPF Page instances and 
customized as needed.

The template XAML and C# code may look quite complex and intimidating, but it looks that 
way so that the pages where the day-to-day work is done may be simpler. The templates 
implement the default appearance and behavior and provide ways to override the defaults 
as well as attach functionality to each instantiation of a template. That way, for each 
individual page, a developer should only have to focus on what that page needs to accomplish.

![Visual Studio XAML Editor](/images/VisualStudio.png?raw=true) 

### Commands
Templates make use of WPF Commands so that developers may add behavior to individual pages 
based on those templates. For example, buttons are disabled by default and take no action 
if they do happen to be enabled. When a page needs to attach behavior to a button on a 
template, it will create command bindings that route to handlers implemented in the page's 
code-behind class.

    <Page.CommandBindings>
        <CommandBinding Command="{x:Static local:Page2.Button1_Command}" Executed="Button1_CommandExecuted" CanExecute="Button1_CommandCanExecute"/>
        <CommandBinding Command="{x:Static local:Page2.Button4_Command}" Executed="Button4_CommandExecuted" CanExecute="Button4_CommandCanExecute"/>
    </Page.CommandBindings>

These commands are then attached to the template's controls.

    <Page.DataContext>
        <local:Template_BasePage 
            PageTitle="Secondary Page" 
            
            Button1_IsEnabled="True" 
            Button1_Content="Back"
            Button1_Command="{x:Static local:Page2.Button1_Command}"
            
            Button4_IsEnabled="True" 
            Button4_Content="Done"
            Button4_Command="{x:Static local:Page2.Button4_Command}"

            Button5_Content="Next"
            />
    </Page.DataContext>

The remaining controls may be ignored if the defaults are acceptable. See 
[Page1.xaml](https://github.com/atmcoin/wpf/blob/main/Page1.xaml) and [Page2.xaml](https://github.com/atmcoin/wpf/blob/main/Page2.xaml)
for full examples.

The behavior of the page is controlled in the page's code-behind file. 

    public partial class Page2 : Page {
        public Page2() {
            InitializeComponent();
        }

        private void basePage_Loaded(object sender, RoutedEventArgs e) {
            // Additional programmatic changes to the template controls may be 
            // performed here.
        }

        public static RoutedCommand Button1_Command = new RoutedCommand();

        private void Button1_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            // This function executes when the command is activated by an associated control 
            // on the template. This example navigates to another page in the application.
            NavigationService.GetNavigationService(this).Navigate(new Uri("Page1.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button1_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            // If this function returns true, the command can be executed, otherwise it is 
            // unavailable. The associated control will change its appearance and availability 
            // as needed.
            e.CanExecute = true;
        }

        public static RoutedCommand Button4_Command = new RoutedCommand();

        private void Button4_CommandExecuted(object sender, ExecutedRoutedEventArgs e) {
            // This function executes when the command is activated by an associated control 
            // on the template. This example navigates to another page in the application.
            NavigationService.GetNavigationService(this).Navigate(new Uri("StartPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button4_CommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
            // If this function returns true, the command can be executed, otherwise it is 
            // unavailable. The associated control will change its appearance and availability 
            // as needed.
            e.CanExecute = true;
        }
    }


## The Template Menagerie

### Start Page
The [StartPage template](https://github.com/atmcoin/wpf/blob/main/Template_StartPage.xaml) 
is an example of a screen that might invite a user to start a transaction or some other interaction. 

![StartPage](/images/Template_StartPage.png?raw=true)

### Base Page
The [BasePage template](https://github.com/atmcoin/wpf/blob/main/Template_BasePage.xaml) 
is an example of a typical screen that might appear more than once during an interaction 
with a kiosk. 

![BasePage](/images/Template_BasePage.png?raw=true)
