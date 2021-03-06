# BeatsMusic.AlbumImporter
========================

This app allows importing albums to your BeatsMusic library from iTunes, Windows Media Player and Rdio. 

This is an open source WPF project demonstrating how to use the BeatsMusic developer APIs from C#.

Go head and read up on Beats Music's Developer APIs @ **https://Developer.BeatsMusic.com**


### Step 1: Choosing which music service to import from 
![Choosing which music service to import from ](http://i.imgur.com/t1XcPpw.jpg)

### Step 2: Login to BeatsMusic using OAuth2 
![Login to BeatsMusic using OAuth2 ](http://i.imgur.com/d4Pu2e3.png)

### Step 3: Provide your Rdio credentials to import Albums
![Provide your Rdio credentials to import Albums](http://i.imgur.com/tMubmib.png)

### Step 4: The app matches your iTunes, Windows Media Player or Rdio albums to BeatsMusic 
![The app matches your iTunes, Windows Media Player or Rdio albums to BeatsMusic ](http://i.imgur.com/jWvbH4E.jpg)

### Step 5: Chillex. 
![Chillex](http://i.imgur.com/jxq44bE.png)


## How to use this project
1. Download the source code 
2. Add your API keys and secrets to BeatsClients.cs and RdioImporter.xaml.cs
3. Build. 
4. Make sure you have .Net 4.5.1 installed.
5. If you're importing from Rdio: run the IE10.reg file in the project base directory. 
7. Open IE10 --> Options --> Security --> Custom Level --> Miscellaneous --> Display Mixed Content --> Change "Prompt" to "Enable".  
8. Run 
9. You may need iTunes / WMP installed as-well. 

I'll add an installer that does all of this soon. 
