# Just.Cash ATM

This solution implements a full-screen kiosk application. It is split into several projects:

* <a id="atmapi">[**AtmApi**](https://github.com/atmcoin/kiosk/tree/main/AtmApi)</a> - The underlying business logic for the application. The output is a standard Windows DLL 
implemented in C++ (targeting [ISO C++20](https://en.wikipedia.org/wiki/C%2B%2B20)) and exposing a C DLL that can be used by most languages that 
target Windows for application development.

* <a id="atmloader">[**AtmLoader**](https://github.com/atmcoin/kiosk/tree/main/AtmLoader)</a> - An application for initially checking and installing dependencies on the kiosk 
hardware and for starting the ATM application once installed. This application is built with .NET Framework 4.7.2 since this is a reliable baseline expected 
to be pre-installed on the kiosk.

* <a id="atmui">[**AtmUi**](https://github.com/atmcoin/kiosk/tree/main/AtmUi)</a> - A [Windows Presentation Foundation (WPF)](https://visualstudio.microsoft.com/vs/features/wpf/) 
application implemented in C# 11 on .NET 7.0. This application provides the ATM user interface, leaving the ATM business logic to the AtmApi project.

* <a id="atmcommon">[**AtmCommon**](https://github.com/atmcoin/kiosk/tree/main/AtmCommon)</a> - A shared project that is built in the context of both AtmLoader and AtmUi. That is, the 
code is actually shared by those two projects, rather than the AtmCommon being built as a library to be shared between those projects. This is because AtmLoader 
and AtmUi are implemented with different versions of .NET and cannot use WPF components from a common assembly. This project is therefore intended to contain UI code 
that is shared between the two projects, such as user controls, templates, converters, view models, and other UI-specific logic that cannot be placed into an assembly 
usable by AtmLoader and AtmUi.

* <a id="atmlib">[**AtmLib**](https://github.com/atmcoin/kiosk/tree/main/AtmLib)</a> - An assembly implementing non-UI code that may be shared between AtmLoader and AtmUi. The project 
includes classes related to tracing, logging, and other non-UI shared logic.

## Building
To build the solution, clone the repository to a folder and open [AtmApp.sln](https://github.com/atmcoin/kiosk/blob/main/AtmApp.sln) in Visual Studio. When you 
build the solution, the [AtmApi](#atmapi) project will automatically copy its DLL output to the [AtmUi](#atmui) target directory.

By default, [AtmUi](#atmui) runs in a 1080 x 1920 full-screen window (portrait HD). If you would instead like to run 
the application in a movable window for testing and debugging, edit the [App.config](https://github.com/atmcoin/kiosk/blob/main/AtmUi/App.config) 
file under AtmUi and change the setting for *IsDevTest* to *True*.

Alternatively, if you are running a multiple-monitor setup, you can move the full-screen application to another monitor using the keyboard. Press 
Windows-Shift-Left or Windows-Shift-Right to move the window to the left-hand or right-hand monitor, as appropriate. For example, in my setup I 
have a 4K landscape monitor as my primary monitor and a portrait HD monitor on the left as my secondary. When I ([Paul](#Paul)) run AtmUi, I press Windows-Shift-Left 
to move the Window, and then I can debug as necessary while seeing the application in its target form factor.

## <a id="templates">Screen Templates</a>

Both AtmUi and AtmLoader are developed on the idea of using screen templates to define common page patterns. These templates are defined in [AtmCommon/Templates](#atmcommon).

* **Base.xaml** - The base template used by all other templates. This defines the overall screen layout, such as the header, footer, and page body.
* **Blank.xaml** - A template for a blank page with no templated controls of its own.
* **Form.xaml** - A template for pages that prompt the user to complete and submit a form.
* **InProgress.xaml** - A template for a transitory page that is shown while a background process is completing. This is in contrast to the [InProgress dialog](#dialog_inprogress).
* **OneButton.xaml** - A template for pages that prompt the user to touch a single button to start a sequence of pages, start an event, confirm an action, etc.
* **OptionScreen.xaml** - A template presenting four buttons that prompt the user to choose an action.
* **OptionScreen8.xaml** - Like OptionScreen.xaml, but presenting eight buttons instead of four.
* **Web.xaml** - A template for presenting a web resource.

Further templates may be created as needed.

New pages may be created from these templates to simplify page creation and to establish a common style throughout the application. Templates are implemented as user controls, and 
the controls are embedded on pages in the page XAML. Controls may then be embedded within the template, if needed.

Template controls are modified using binding, not by naming individual controls. This minimizes coupling between templates and their underlying pages.

Template binding is controlled through view models. Base view models for each template are provided in the Templates folder of the [AtmCommon](#atmcommon) project. Each page 
may then extend the view model for its own purposes, if desired.

See DemoPage.xaml and DemoPage.xaml.cs in the [AtmCommon](#atmcommon) project for examples of how to use a template. The page links to other such demos.

## <a id="controls">Controls</a>

The [AtmCommon](#atmcommon) project implements a few user controls.

* **Dialog_InProgress.xaml** - Present a modal dialog above the current page that indicates a background process is executing. The dialog may enclose other controls if necessary, 
such as a progress bar. See ProgressDialogSample.xaml in the [AtmLoader](#atmloader) project for an example of how to use the control.

* **Dialog_OkCancel.xaml** - Present a modal dialog with two options to the user. See SystemPage.xaml in the [AtmLoader](#atmcommon) project for an example of how to use the control.

* **FixedNavigation.xaml** - Define the common navigation buttons that may appear on each page template. These may be bound to page-specific commands or may use default commands. They 
may also be hidden via a bound property setting, if desired.

* **TouchKeyboard.xaml** - Present a keyboard control to the user. The user may type on the touch keyboard to fill data into designated on-screen fields. See KeyboardDemo.xaml in 
the [AtmLoader](#atmloader) project for an example of how to use the control.

## Logging/Tracing

Logging and tracing is accomplished via the tracing features implemented in the [.NET System.Diagnostics namespace](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics?view=net-7.0). 
There is a thin wrapper around the [Trace](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.trace?view=net-7.0) class implemented in Tracing/Logger.cs in the 
[AtmLib](#atmlib) project.

Traces are written to the AppData directory of the Windows user under which the ATM applications run. For example, the AtmLoader application will log to 
*C:\Users\<username>\AppData\Local\Merapi\loader\AtmLoader.log*. 

(Original edits by [Victor](#Victor))

Here are some specs/suggestions for the templates and app architecture.  

I think we will need __four__ templates: Home, Base, Form (a screen with a bunch of text fields to get input), and Web (a template that goes to an online web page if we need that for something.

### Screen Hardware

Posiflex PCAP PoS touchscreen
1920x1080 resolution
portrait mode

### Background Color

The kiosk boxes are red and white.   We will use as the screen background a purplish navy blue, a color I call "Gulf Stream" based on the deep water off of Florida's east coast.

### Header and Footer

Header and Footer will look cool in the template.  Footer can be a brand icon placeholder (TBD) and some contact info on the screen at the bottom, maybe a clock and timezone header, also showing current location of kiosk, including Lat/Lon.

### Buttons

At the top of the portrait screen, 4 buttons with plenty of space for text and icons arranged at the top of the screen, screen 1 looks like this:

      1. PURCHASE BTC                2. Register (Phone KYC)

      3. REDEEM BTC                  4. Register (Enhanced KYC)   

screen 2.1 (*2.1 is what you get when you hit button 1)  looks like this:

      1. Cash to Paper Wallet        2. Debit Card to Paper Wallet

      3. Cash to BTC Address         4. Debit Card to BTC Address

screen 2.2:

This will use the "Form" template where we ask the customers to fill out a bunch of things on the screen

screen 2.3:

Use base template.

screen 2.4:

Form template.

... and so on...

__Summary__: we need 4 templates, 1920 x1080, all with blue background and header and footers. Create screens as above based on the templates. Wire everything together and establish document naming conventions and coding conventions.


## TEAM CONTACTS

### <a id="Victor">Victor Cook</a>
Owner

Email: [victor@just.cash](victor@just.cash)<br/>
Telegram: 954-397-9859<br/>
Whatsapp 561-351-1280<br/>
Skype: victorjcook<br/>

### <a id="Dzmitry">Dzmitry</a>
Part time, working on VS2022 Project.

Email: [jperrot2082@gmail.com](mailto:jperrot2082@gmail.com)<br/>
Skype: live:.cid.b60cfdfc23fb6dfe<br/>

### <a id="Paul">Paul Parks</a>
Working on UI and API structure

Email: [paul@parkscomputing.com](mailto:paul@parkscomputing.com)<br/>
Mobile, [WhatsApp](https://wa.me/6584363982): +65 8436 3982<br/>
US VOIP: +1 678 575 8255<br/>

