# Twig for .NET

The project provides PHP's Twig library for .NET.

The Twig library is developed at https://github.com/twigphp/Twig/

## Basic usage

```razor
@{
    ViewData["Title"] = "My Twig Page";
}

@Html.Twig("/path/to/templates", "index.html", new { name = "John Doe", });
```

## Repository structure

- **Twig.AspNetCore**: provides helpers for Razor pages.
- **Twig.Twig**: contains the Twig library.

## How it works

The unmodified PHP code is compiled to .NET assembly using [PeachPie PHP Compiler](http://github.com/peachpiecompiler/peachpie). Resulting library can be seamlessly referenced by other .NET projects (C#, VB.NET, F#), used for rendering Twig e.g. within ASP.NET Core pages.
