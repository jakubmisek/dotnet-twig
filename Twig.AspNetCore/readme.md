The project provides Twig templates parsing and rendering for .NET.

The sources of original library are maintained at https://github.com/twigphp/Twig/.

### Basic usage

**Render Twig template in a razor page**

<center>Index.cshtml</center>

```razor
@{
    ViewData["Title"] = "My Twig Page";
}

@Html.Twig("/path/to/templates", "index.html", new { name = "John Doe", });
```

<center>path/to/templates/index.html</center>

```html
<p>
    Hello {{ Name }}!
</p>
```

### Template data

The last argument specifies the template data. It can be any `object`, `PhpArray`, `IDictionary` or `null`. In case it is ommited or `null`, the page's `ViewData` are used implicitly.

### How it works

The unmodified PHP code is compiled to .NET assembly using [PeachPie PHP Compiler](http://github.com/peachpiecompiler/peachpie). Resulting library can be seamlessly referenced by other .NET projects (C#, VB.NET, F#), used for rendering Twig e.g. within ASP.NET Core pages. The library does not need PHP or a PHP server.
