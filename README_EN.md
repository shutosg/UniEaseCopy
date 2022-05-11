# UniEaseCopy

UniEaseCopy is an Editor Tool that allows you to copy and paste Unity Animation keyframes while preserving the curve shape.

## Demo

![UniEaseCopy](https://user-images.githubusercontent.com/6266016/167908285-3bf3886d-7b09-40b3-b1f9-726ced2ea2cf.gif)

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

You can also control the menu by adding `UNIEASECOPY_USE_MENU_ITEM` to `Scripting Define Symbols` in PlayerSettings.

![image](https://user-images.githubusercontent.com/6266016/167912822-51d1789c-ce40-4de1-a208-99a73e2deec6.png)

## Installation

You can add it to your project from PackageManager.

### from git URL

![image](https://user-images.githubusercontent.com/6266016/167906590-0358137f-83bc-4d5a-981f-6eb867c261c9.png)

You can install it by entering `https://github.com/shutosg/UniEaseCopy.git?path=Assets/UniEaseCopy` in the `Add package from git URL` of PackageManager.

## License

MIT
