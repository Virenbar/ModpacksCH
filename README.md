# ModpacksCH [![Build artifact](https://img.shields.io/github/actions/workflow/status/Virenbar/ModpacksCH/build-artifact.yml?label=Build&logo=github)](https://github.com/Virenbar/ModpacksCH/actions/workflows/build-artifact.yml)

CLI tool for downloading modpacks from [Feed the Beast](https://www.feed-the-beast.com/modpacks) and [CurseForge](https://www.curseforge.com/minecraft/modpacks) using modpacks.ch

![terminal](/assets/images/terminal.gif)

```text
.\ModpacksCH <command> [options]

Commands:
    s, search <name>                       Search modpacks by name
    i, info <modpackID>                    Show info about modpack
    d, download <modpackID> [<versionID>]  Download modpack

Options:
    -s, --server       Download server version
    -p, --path <path>  Directory to save modpack [default: <current-dir>]
    -t, --trace        Write trace log
    -?, -h, --help     Show help and usage information
```

## Installation

* Install [.NET 6.x](https://dotnet.microsoft.com/download)
* Download [latest release](https://github.com/Virenbar/ModpacksCH/releases)

## Usage

1. First search for modpack ID

    ```text
    > .\ModpacksCH search <name>

    Example:
    > .\ModpacksCH s stoneblock
                               FTB Modpacks
    ┌─────┬───────────────────────────┬─────────┬────────────┐
    │ ID  │ Name                      │ Version │ MC Version │
    ├─────┼───────────────────────────┼─────────┼────────────┤
    │ 4   │ FTB Presents Stoneblock 2 │ 1.22.0  │ 1.12.2     │
    │ 100 │ FTB StoneBlock 3          │ 1.2.1   │ 1.18.2     │
    └─────┴───────────────────────────┴─────────┴────────────┘
    ...
    ```

2. Then available modpack versions

    ```text
    > .\ModpacksCH info <modpackID>

    Example:
    > .\ModpacksCH i 100
    Modpack: FTB StoneBlock 3 (ID: 100)
    ┌───────────────────────────────────────────────────────Synopsis───────────────────────────────────────────────────────┐
    │ In a world surrounded by stone, build yourself a subterranean kingdom that really rocks! Use magic and technology to │
    │ forge your realm to your designs.                                                                                    │
    └──────────────────────────────────────────────────────────────────────────────────────────────────────────────────────┘
    Latest version: ID: 2287 - 1.2.1 (Beta)
    Other versions
    ├── ID: 2283 - 1.2.0 (Release)
    ├── ID: 2282 - 1.1.1 (Release)
    ...
    ```

3. Download modpack (Latest version if no version ID provided)

    ```text
    > .\ModpacksCH download <modpackID> [<versionID>]

    Example:
    > .\ModpacksCH d 100
    ```

### Notes

* You can also find modpack and version ID on [modpack page](https://www.feed-the-beast.com/modpacks)
* note.txt contains Forge version and recommended memory
* Executing without arguments will launch in interactive mode
* Windows 7 not supported (modpack.ch supports only TLS 1.3)
* Tested with Stoneblock 3 and ATM 8
