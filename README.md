![Zombie Logo](http://2toad.com/areas/project/content/images/zombie-logo-small.png)

Zombie is an opinionated fullstack ASP.NET solution. Built with best practices, popular frameworks, and tasty *BRAINS*, it's designed to support all your wildest dreams (or at the very least: your web development needs).

## WARNING * WARNING * WARNING ##

> Zombie is currently in pre-alpha. I don't recommend using it until a release has been published. Until a release is published: documentation will be practically nonexistent, bugs will abound, features will be incomplete, breaking changes will occur frequently, and whales will fall from the sky!

## Technology Stack ##

### Frontend ###

* HTML5
* AngularJS
* Bootstrap 3
* Font Awesome
* Lo-Dash
* Sass

### Backend ###

* C# (Microsoft .NET 4.5.1)
* ASP.NET MVC 5 (Web API 2)
* Autofac
* Barricade (OAuth2)
* EF 6 (Code First + Migrations)
* SQL Server 2012
* Elmah

### Developer Tools ###

* Yeoman
* Gulp
* Bower
* BrowserSync

## Getting Started ##

### Yeoman ###

The best way to scaffold your new application is to use the Yeoman generator we created for Zombie.

1. Open a command prompt
1. Install yeoman: `npm install -g yo`
1. Install the Zombie generator: `npm install -g generator-zombie`

> If the above commands look foreign to you, you're probably not familiar with Node's package manager, NPM. If that's the case, don't worry, just start by installing [Node.js](https://nodejs.org/), then return to this guide.

### Create your application ###

1. Open a command prompt
1. Start the generator: `yo zombie`
1. Follow the generator's instructions

### Build your application ###

Zombie requires the following developer tools to be installed on your system before you can build the application:

* Visual Studio 2013 (Update 4)
* SQL Server 2012 (Express or better)

**Step 1:** build your application in Visual Studio, then host it in IIS Express:

1. Open your solution in Visual Studio
1. Change the default configuration settings:
    * AppSettings.config
    * ConnectionStrings.config
    * Elmah.config
1. Build the solution (this will also install required NuGet packages): <kbd>F6</kbd>
1. Start your application in IIS Express: <kbd>Ctrl</kbd>+<kbd>F5</kbd> (or <kbd>F5</kbd> if you want to debug)

> Visual Studio will open your application in a browser at http://localhost:65432. Close the browser after it opens (you'll see why in the next step).

**Step 2**: launch the application using Gulp:

1. Open a command prompt
1. Switch to your applications root directory (where gulpfile.js is located)
1. Run the default Gulp task: `gulp`

> Gulp will use BrowserSync to open your application in a browser at http://localhost:3000. BrowserSync will watch for changes to your application's HTML, JavaScript, Sass, and .NET assembly. When a change to these monitored files is detected, BrowserSync will compile your Sass into CSS and refresh your browser automatically.

## License ##

Zombie is licensed under [The MIT License](https://github.com/2Toad/generator-zombie/blob/master/LICENSE), giving you the freedom to use it in both commercial and non-commercial applications.

Copyright &copy;2015 [2Toad, LLC](http://2toad.com)
