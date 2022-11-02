# UniEaseCopy Document

UniEaseCopy is an Editor Tool that allows you to copy and paste Unity Animation keyframes while preserving the curve shape.

## Contents

1. Demo
2. Usage
3. Installation
4. License

## Demo

[Demonstration](https://user-images.githubusercontent.com/6266016/167908285-3bf3886d-7b09-40b3-b1f9-726ced2ea2cf.gif)

## Usage

There are two ways to use this tool.

### 1. GUI panel

Open the GUI panel from the menu `Window > Animation > UniEaseCopy`.

![image](https://user-images.githubusercontent.com/6266016/167912237-8625964e-b092-499b-b33c-4fe9e2c684c6.png)

![image](https://user-images.githubusercontent.com/6266016/167908621-62d97187-5fbb-4c36-bbd8-6e33e9847aff.png)

The explanations of the panel are following.

||Explanation|
|:--|:--|
|Copy button|Copy the currently selected keyframes|
|Ease button|Pastes the curve shape to the currently selected keyframes|
|Value button|Paste the value to the selected keyframes|
|Log area|Displays error messages etc.|
|Copied keyframes|Displays the number of keyframes being copied. Some Unity versions also display information about copied keyframes.|

### 2. Menu

You can also show the menu by adding `UNIEASECOPY_USE_MENU_ITEM` to `Scripting Define Symbols` in PlayerSettings.

![image](https://user-images.githubusercontent.com/6266016/167912822-51d1789c-ce40-4de1-a208-99a73e2deec6.png)

## Installation

There are 3 ways to add this package to your project.

### 1. Scoped registory

By adding the following scoped registory from `PackageManager` in ProjectSettings, this package will appear in `My Registories` in PackageManager.

![image](https://user-images.githubusercontent.com/6266016/168285937-41510ac6-bbd8-4bf8-88e8-ecdb8a0aecb2.png)

```json
{
    "name": "shutosg",
    "url": "https://package.openupm.com",
    "scopes": [
        "net.shutosg"
    ]
}
```

![image](https://user-images.githubusercontent.com/6266016/168287158-616e7faa-4b10-42c7-abcc-0f47e7172d54.png)

### 2. OpenUPM

You can add this package using the [OpenUPM](https://openupm.com/) CLI tool.

```shell
openupm add net.shutosg.uni-ease-copy
```

### 3. from git URL

![image](https://user-images.githubusercontent.com/6266016/167906590-0358137f-83bc-4d5a-981f-6eb867c261c9.png)

You can install it by entering `https://github.com/shutosg/UniEaseCopy.git?path=Assets/UniEaseCopy` in the `Add package from git URL` of PackageManager.

## License

MIT
