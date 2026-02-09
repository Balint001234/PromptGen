# AI Prompt Optimizer

Quick desktop app for turning rough prompts into better structured ones using Gemini.

## What it does

Takes your messy prompt ideas and restructures them into proper format using Google's Gemini API. You can choose output language (Hungarian, English, German, French, Spanish).

## You need

- .NET 10.0
- Your own Gemini API key - get it from https://aistudio.google.com/app/apikey

## Setup

```bash
git clone https://github.com/Balint001234/PromptGen.git
cd PromptGen
dotnet build
dotnet run
```

First time you run it, click the settings (⚙️) button and add your API key.

## Usage

1. Pick your output language
2. Type your rough prompt
3. Hit generate
4. Copy the result

## Settings file

Your API key gets saved in `settings.json` (without encrypted ). By default it's in the same folder as the exe.

**Important:** Don't share this file or commit it to git. It's already in .gitignore but just saying.

You can change where settings are saved in the settings menu if you want.

## Disclaimer

This is a personal project. Use at your own risk.

- I'm not responsible for your API costs
- I'm not responsible if your API key gets stolen
- I'm not responsible for anything really
- Secure your own stuff
- Check Google's ToS for their API



## Tech

Built with Avalonia UI and C#. Uses Google's Gemini API through their .NET SDK.

---

Remember to keep your API key safe and monitor your usage in Google Cloud Console.
