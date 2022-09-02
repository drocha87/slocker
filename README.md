# SLocker

This PWA is used to organized my environment variables in my development machine. It will store the key value pairs in the `indexeddb` **without any encryption**.

## How to use

```shell
git clone ...
cd ...
dotnet publish -c Release -o ./publish
cd ./publish/wwwroot
python3 -m http.server 6969
```

Open your browser on `http://localhost:6969` and install the app (_on edge and chrome you will see a small + icon on the navigation bar_) if you wish.

## Motivation

I create this PWA just to play with blazor wasm and PWA. Also I would like to play a little bit with `indexeddb` so this was the only project that came to my mind at the moment.
