# flatZ
FlatZ (aka Focus Fixer) - tool to create Z-projections from moving intravital imaging

# How to install
As with other tools, there is a compiled installer available here: https://www.dropbox.com/sh/r6hy1yfg8zcbp7y/AAAQm0n7ZEx9-fpQhry_253ua?dl=0. 
You'll need .NET v4.5.2 or above. 

# What does it do
Often when capturing intravital imaging, the tissue or area you are looking at is not perfectly flat. In order to properly examine the area, the most frequent approach is to generate a max Z-projection, which gives you an idea of what the area would have appeared like if it had of been flat. In tissues or sections which are still, generating a Z-projection is simple - and many tools are able to do this. However, in tissues which are moving, Z-projections often develop artifacts such as blurring as pixels move in relation to each other on a frame to frame basis. 
FlatZ allows you to build composite images by a) selecting in focus image segments (whether that's in terms of movement or being in planar focus) elements to contribute to a composite resulting image, and b) perform slight adjustments to move overlay images to compensate for movement. It is not automated, and does require a user to generate the Z-projection. 
The source images can be obtained from almost any running application, provided that it isn't covered by another application and the window is maximised. 

# How does it work
- Select the source application
  - To tell the software where to obtain the source images from, click on the checkbox next to Find Window and move the mouse over the image in your applicatio of choice. *Don't click the window* but press space to remove the tickbox. This will tell the software the window you want to capture. 
- Choose the image colour
  - Choose a colour to match the colour of the image being used. You can only use Green, Red or Blue at the moment. 
- Capture the images 
  - Press capture to obtain the first image. In your software application, move the image onto a new view and press Capture in the second window. Now you have two images, you can composite them. This will generate a max projection of these two frames, with the resulting image consisting of the maximal pixels from each image. Once you're happy, you can move the composite to the first image holder by choosing "Copy to Image 1". Once you have another good position, click on Capture in the second window and generate a new composite. Continue until finished!
- Nudge if required 
  - You can move the image slightly if it is noticably shifted in either X or Y. You should only really use this if it is clear that you can align the images accurately (e.g. if there the same structure is visible on both images and you can align them properly. 
