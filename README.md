# VR Search and Rescue

This project is a proof-of-concept for search and rescue (SAR) training in virtual reality from the Vision Sciences and Memory Lab at New Mexico State University. It was developed primarily for use with a Meta Quest 2 headset.

**Authors:** Paean Luby, Ashley Mathis, Bryan White, Rebecca Penn, Arryn Robbins, Michael C. Hout
![figure](https://github.com/user-attachments/assets/924b8b66-b410-4986-80e9-747dd03c9965)

## Experiment

To replicate the experiment conducted by our lab, check out [these](https://docs.google.com/document/d/1Hck5YElchSHHaskutDsmCjW0eLxDlkaxncg6RpyC6LA/edit?usp=sharing) participant directions.

## Data Collected

Time to find the object in each trial (ms), similarity of the object to the background (quantified via mean squared error), object distance from the player, object and player positions, and object type are outputted for each player at the end of block. All data from the trials will be in the "Experiments" folder of the "Assets" folder.


## Installation- Unity

1. Open the project in Unity.
2. Download and open [Meta Quest Link](https://www.meta.com/help/quest/articles/headsets-and-accessories/oculus-rift-s/install-app-for-link/).
3. Press the "Play" button at the top of the screen. Connect the headset via Air Link or Link Cable. If you don't connect a headset, then the program will run in desktop mode, which doesn't have interactive controls.

## Installation- Steam

1. Download Steam.
2. From Unity, go to "Build Settings" and export a web version of game. While the game can be downloaded for the headset locally, no data will be recorded.
3. Add the non-Steam game for your library and press play. 

More detailed directions for playing in Steam can be accessed [here](https://docs.google.com/document/d/1zakldh99gSuYRKAK3fwoAfnc6K3CaH-al-TsETnB3ho/edit?usp=sharing).

## Contributing

Adding functionality to the game or additional measurements is always welcome. Contact paean.luby@richmond.edu with any questions about contributing.

## Acknowledgements

Thank you to the creators of [sXR](https://github.com/simpleOmnia/sXR.git) for streamlining implementing the experiment in the VR. 

## License

[MIT](https://choosealicense.com/licenses/mit/) participant directions. Additional resources will be supplied upon request.
