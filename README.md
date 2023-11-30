# Flow-Launcher WireGuard Plugin

This plugin allows to establish a connection with a [WireGuard](https://www.wireguard.com/) VPN tunnel via [FlowLauncher](https://www.flowlauncher.com/).


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

To install and use this plugin, you will need the following:

- [FlowLauncher](https://www.flowlauncher.com/)
- [WireGuard Windows Client](https://www.wireguard.com/install/#windows-7-81-10-11-2008r2-2012r2-2016-2019-2022)

Please make sure you have the latest versions of both installed before proceeding.

### Installing

#### FlowLauncher

![Install](assets/installation.png)

Run `pm install WireGuard` in FlowLauncher to install this plugin

#### Manual
Unzip [the latest version](https://github.com/flooxo/Flow.Plugin.WireGuard/releases) into your plugin directory (default: `%appdata%\FlowLauncher\Plugins` or run `userdata` in FlowLauncher)

## Configuration

| Setting     | Type    | Default                                         | Description             |
|-------------|---------|-------------------------------------------------|-------------------------|
| `config path` | string  | `C:\Program Files\WireGuard\Data\Configurations\` | path to your WireGuard tunnel configurations |

## Usage

- actionword: `vpn`

![Example](assets/usage.png)

When the actionword is called, it displays all the available VPN tunnels from the configuration.
The user can select a tunnel to connect to it. If the tunnel is already connected, selecting it again will disconnect the connection.
If the user already has an active VPN connection, selecting a new tunnel will switch the connection to the new tunnel.


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
