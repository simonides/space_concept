using UnityEngine;

public class Number : MonoBehaviour {

    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite four;
    public Sprite five;
    public Sprite six;
    public Sprite seven;
    public Sprite eight;
    public Sprite nine;
    public Sprite ten;

    public Sprite GetSpriteForNumber(int num)
    {
        //Debug.Log("Sprite num : " + num);
        Debug.Assert(num >= 0 && num <= 10);
        switch (num){
            case 1:     return one;
            case 2:     return two;
            case 3:     return three;
            case 4:     return four;
            case 5:     return five;
            case 6:     return six;
            case 7:     return seven;
            case 8:     return eight;
            case 9:     return nine;
            default:    return ten;
        }
    }
}
