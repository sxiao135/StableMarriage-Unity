//change the color based on the input size
//go around the color wheel
// rgb: preferably btwn 100-255
//The hex code circle works by increasing r, reaching 255, increasing g, reaching 255, 
// decreasing r to 0, then increasing b to 225, an then finally decreasing g to 0
// try instead of increasing/decreasing around 0, have a different middle (like 14 or 153 or sth)

public class ColorPicker {
    // private int red = 0, green = 0, blue = 0;
    private Vector3 RGB = new Vector3(0,0,0); //r = 1, g = 2, b = 3
    private eqPoint = new int;
    private void setUp(){ //hoping for input as the # in line of nurse
        //this for loop is temporary
        Random rnd = new Random();
        int red, green, blue;
        //pick r, g, or b
        int start = rnd.Next(1,3);
        eqPoint = rnd.Next(100, 255) //colors below 100 not great, 255 is max saturaion

        if (start == 1){
            //red start
        }
        else if (start == 2){
            //green start
        }
        else if (start ==3){
            //blue start
        }
    }

    public void chooseColor(){
        for (int i = 0; i < 10; i++){ //if there was only 10 objects

        }
    }
}
