## Application
Console application that uses NASA API described here (https://api.nasa.gov/) to build a project in GitHub that calls the Mars Rover API and selects a picture on a given day.
1.  If the text file doesn't exists , it creates a file (nasa.txt) inside the project folder directory that saves 4 dates in the correct format accepted by the api.- I assumed your local doesn't have that file on first load.
2. It reads the dates from nasa.txt and passes it as param for the api url.
> If the date is invalid, it will not call api.
3. Once api is called, it returns JSON data which the app fill parse as NASAData obj.
4. The program also saves the images into local directory in C:\Users\nasa_imgs
5. The program also browse the images in chrome browser, if chrome is not found, it will open default browser which is edge (used to be iexplorer).

## TEST
Unit test is to check whether the string dates given were correctly parsed and written in nasa.txt
It also checks that the files were dowloaded in the given file path


##Docker

dockerfile added to run in docker container