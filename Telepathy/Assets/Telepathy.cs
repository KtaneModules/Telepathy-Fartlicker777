using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class Telepathy : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public KMSelectable[] Move;
   public KMSelectable[] Submits;

   public TextMesh[] Coordinates;
   public GameObject CoordinateDisplay;
   public Material[] CoordinateColorsAndShapes;

   public SpriteRenderer TheSymbols;
   public Sprite[] TheSymbolsButNotRenderer;

   public Renderer[] Republicans;

   /*
       blue		1
       pink		2
       green		3
       brown		4
       yellow		5
       purple		6
       vibrator pink	7
       orange		8
       grey		9
       bolt	1
       dice	2
       gem		3
       hand	4
       moon	5
       eye		6
       star	7
       heart	8
       sun		9
   */
   //
   int[][] Grid = new int[][] {
      new int[] {11, 22, 33, 44, 55, 66, 77, 88, 99, 36, 97, 42, 75, 69, 51, 83, 14, 28}, //a
      new int[] {24, 38, 96, 82, 41, 79, 15, 63, 57, 64, 78, 27, 53, 92, 16, 31, 49, 85}, //b
      new int[] {98, 56, 47, 73, 62, 25, 34, 19, 81, 57, 41, 63, 24, 15, 82, 79, 38, 96}, //c
      new int[] {46, 87, 61, 29, 13, 94, 58, 35, 72, 23, 39, 54, 67, 86, 95, 48, 71, 12}, //d
      new int[] {67, 71, 12, 95, 39, 48, 86, 54, 23, 99, 55, 88, 11, 77, 44, 66, 22, 33}, //e
      new int[] {32, 93, 59, 68, 84, 17, 21, 76, 45, 81, 62, 19, 98, 34, 73, 25, 56, 47}, //f
      new int[] {75, 14, 28, 51, 97, 83, 69, 42, 36, 45, 84, 76, 32, 21, 68, 17, 93, 59}, //g
      new int[] {89, 65, 74, 37, 26, 52, 43, 91, 18, 72, 13, 35, 46, 58, 29, 94, 87, 61}, //h
      new int[] {53, 49, 85, 16, 78, 31, 92, 27, 64, 18, 26, 91, 89, 43, 37, 52, 65, 74}, //i
      new int[] {25, 34, 19, 62, 81, 47, 73, 56, 98, 49, 64, 78, 92, 85, 53, 16, 27, 31}, //j
      new int[] {94, 58, 35, 13, 72, 61, 29, 87, 46, 14, 36, 97, 69, 28, 75, 51, 42, 83}, //k
      new int[] {52, 43, 91, 26, 18, 74, 37, 65, 89, 71, 23, 39, 86, 12, 67, 95, 54, 48}, //l
      new int[] {48, 86, 54, 39, 23, 12, 95, 71, 67, 22, 99, 55, 77, 33, 11, 44, 88, 66}, //m
      new int[] {31, 92, 27, 78, 64, 85, 16, 49, 53, 65, 18, 26, 43, 74, 89, 37, 91, 52}, //n
      new int[] {66, 77, 88, 55, 99, 33, 44, 22, 11, 93, 45, 84, 21, 59, 32, 68, 76, 17}, //o
      new int[] {17, 21, 76, 84, 45, 59, 68, 93, 32, 87, 72, 13, 58, 61, 46, 29, 35, 94}, //p
      new int[] {79, 15, 63, 41, 57, 96, 82, 38, 24, 56, 81, 62, 34, 47, 98, 73, 19, 25}, //q
      new int[] {83, 69, 42, 97, 36, 28, 51, 14, 75, 38, 57, 41, 15, 96, 24, 82, 63, 79} //r
    };
   int[] UsedSymbols = new int[4];
   int[] RandomAss = { 0, 1, 2, 3 };
   int[] Unicorn = { 0, 84, 260, 230 };
   int Timwi = 0;
   int Index = 0;
   int ColorIndex = 0;

   int SolutionForTPAuto;

   byte[][] Colors = new byte[][] {
      new byte[] {3, 158, 224},//blue
      new byte[] {249, 153, 181},//pink
      new byte[] {37, 180, 72},//green
      new byte[] {142, 62, 11},//brown
      new byte[] {250, 210, 0},//yellow
      new byte[] {155, 77, 161},//purple
      new byte[] {222, 19, 82},//red
      new byte[] {245, 118, 25},//orange
      new byte[] {161, 170, 179},//silver
    };

   private List<string> Help = new List<string> { "0123", "0132", "0213", "0231", "0312", "0321", "1023", "1032", "1203", "1230", "1302", "1320", "2013", "2031", "2103", "2130", "2301", "2310", "3012", "3021", "3102", "3120", "3201", "3210" };
   string[] ColorsForLogging = { "Blue", "Pink", "Green", "Brown", "Yellow", "Purple", "Red", "Orange", "Silver" };
   string[] SymbolsForLogging = { "Bolt", "Dice", "Gem", "Hand", "Moon", "Eye", "Star", "Heart", "Sun" };
   string Alphabet = "ABCDEFGHIJKLMNOPQR";
   string Weed = "";
   string Crack = "";

   bool Autosolving;

   static int moduleIdCounter = 1;
   int moduleId;
   private bool moduleSolved;

   void Awake () {
      moduleId = moduleIdCounter++;
      foreach (KMSelectable Moon in Move) {
         Moon.OnInteract += delegate () { MovePress(Moon); return false; };
      }
      foreach (KMSelectable Submit in Submits) {
         Submit.OnInteract += delegate () { SubmitPress(Submit); return false; };
         Submit.OnHighlight += delegate () { SquareHighlight(Submit); };
         Submit.OnHighlightEnded += delegate () { SquareHighlightGone(Submit); };
      }
   }

                                                                                                                                                                                                                                                                                             void Start () {
                                                                                                                                                                                                                                                                                                TheSymbols.GetComponent<SpriteRenderer>().sprite = TheSymbolsButNotRenderer[Index];
                                                                                                                                                                                                                                                                                                TheSymbols.GetComponent<SpriteRenderer>().color = new Color32(Colors[ColorIndex][0], Colors[ColorIndex][1], Colors[ColorIndex][2], 255);
                                                                                                                                                                                                                                                                                                if (ColorIndex == 0) {
                                                                                                                                                                                                                                                                                                   Republicans[0].material.color = new Color32(Colors[ColorIndex + 1][0], Colors[ColorIndex + 1][1], Colors[ColorIndex + 1][2], 255);
                                                                                                                                                                                                                                                                                                   Republicans[1].material.color = new Color32(Colors[8][0], Colors[8][1], Colors[8][2], 255);
                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                else if (ColorIndex == 9) {
                                                                                                                                                                                                                                                                                                   Republicans[0].material.color = new Color32(Colors[0][0], Colors[0][1], Colors[0][2], 255);
                                                                                                                                                                                                                                                                                                   Republicans[1].material.color = new Color32(Colors[ColorIndex - 1][0], Colors[ColorIndex - 1][1], Colors[ColorIndex - 1][2], 255);
                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                else {
                                                                                                                                                                                                                                                                                                   Republicans[0].material.color = new Color32(Colors[ColorIndex + 1][0], Colors[ColorIndex + 1][1], Colors[ColorIndex + 1][2], 255);
                                                                                                                                                                                                                                                                                                   Republicans[1].material.color = new Color32(Colors[ColorIndex - 1][0], Colors[ColorIndex - 1][1], Colors[ColorIndex - 1][2], 255);
                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                Restart:
                                                                                                                                                                                                                                                                                                UsedSymbols[0] = UnityEngine.Random.Range(0, Grid.Length) * 18 + UnityEngine.Random.Range(0, Grid.Length);
                                                                                                                                                                                                                                                                                                do {
                                                                                                                                                                                                                                                                                                   UsedSymbols[1] = UnityEngine.Random.Range(0, Grid.Length) * 18 + UnityEngine.Random.Range(0, Grid.Length);
                                                                                                                                                                                                                                                                                                } while (
                                                                                                                                                                                                                                                                                              (UsedSymbols[1] / 18 == UsedSymbols[0] / 18) ||
                                                                                                                                                                                                                                                                                              (UsedSymbols[1] % 18 == UsedSymbols[0] % 18) ||
                                                                                                                                                                                                                                                                                              (Grid[UsedSymbols[1] / 18][UsedSymbols[1] % 18] / 10 == Grid[UsedSymbols[0] / 18][UsedSymbols[0] % 18] / 10) ||
                                                                                                                                                                                                                                                                                              Grid[UsedSymbols[1] / 18][UsedSymbols[1] % 18] % 10 == Grid[UsedSymbols[0] / 18][UsedSymbols[0] % 18] % 10);
                                                                                                                                                                                                                                                                                                do {
                                                                                                                                                                                                                                                                                                   UsedSymbols[2] = UnityEngine.Random.Range(0, Grid.Length) * 18 + UnityEngine.Random.Range(0, Grid.Length);
                                                                                                                                                                                                                                                                                                } while (
                                                                                                                                                                                                                                                                                              (UsedSymbols[2] / 18 == UsedSymbols[0] / 18) ||
                                                                                                                                                                                                                                                                                              (UsedSymbols[2] % 18 == UsedSymbols[0] % 18) ||
                                                                                                                                                                                                                                                                                              (Grid[UsedSymbols[2] / 18][UsedSymbols[2] % 18] / 10 == Grid[UsedSymbols[0] / 18][UsedSymbols[0] % 18] / 10) ||
                                                                                                                                                                                                                                                                                              Grid[UsedSymbols[2] / 18][UsedSymbols[2] % 18] % 10 == Grid[UsedSymbols[0] / 18][UsedSymbols[0] % 18] % 10 ||
                                                                                                                                                                                                                                                                                              (UsedSymbols[2] / 18 == UsedSymbols[1] / 18) ||
                                                                                                                                                                                                                                                                                              (UsedSymbols[2] % 18 == UsedSymbols[1] % 18) ||
                                                                                                                                                                                                                                                                                              (Grid[UsedSymbols[2] / 18][UsedSymbols[2] % 18] / 10 == Grid[UsedSymbols[1] / 18][UsedSymbols[1] % 18] / 10) ||
                                                                                                                                                                                                                                                                                              Grid[UsedSymbols[2] / 18][UsedSymbols[2] % 18] % 10 == Grid[UsedSymbols[1] / 18][UsedSymbols[1] % 18] % 10);
                                                                                                                                                                                                                                                                                                do {
                                                                                                                                                                                                                                                                                                   UsedSymbols[3] = UnityEngine.Random.Range(0, Grid.Length) * 18 + UnityEngine.Random.Range(0, Grid.Length);
                                                                                                                                                                                                                                                                                                } while (
                                                                                                                                                                                                                                                                                                (UsedSymbols[3] / 18 == UsedSymbols[0] / 18) ||
                                                                                                                                                                                                                                                                                                (UsedSymbols[3] % 18 == UsedSymbols[0] % 18) ||
                                                                                                                                                                                                                                                                                                (Grid[UsedSymbols[3] / 18][UsedSymbols[3] % 18] / 10 == Grid[UsedSymbols[0] / 18][UsedSymbols[0] % 18] / 10) ||
                                                                                                                                                                                                                                                                                                Grid[UsedSymbols[3] / 18][UsedSymbols[3] % 18] % 10 == Grid[UsedSymbols[0] / 18][UsedSymbols[0] % 18] % 10 ||
                                                                                                                                                                                                                                                                                                (UsedSymbols[3] / 18 == UsedSymbols[1] / 18) ||
                                                                                                                                                                                                                                                                                                (UsedSymbols[3] % 18 == UsedSymbols[1] % 18) ||
                                                                                                                                                                                                                                                                                                (Grid[UsedSymbols[3] / 18][UsedSymbols[3] % 18] / 10 == Grid[UsedSymbols[1] / 18][UsedSymbols[1] % 18] / 10) ||
                                                                                                                                                                                                                                                                                                Grid[UsedSymbols[3] / 18][UsedSymbols[3] % 18] % 10 == Grid[UsedSymbols[1] / 18][UsedSymbols[1] % 18] % 10 ||
                                                                                                                                                                                                                                                                                                (UsedSymbols[3] / 18 == UsedSymbols[2] / 18) ||
                                                                                                                                                                                                                                                                                                (UsedSymbols[3] % 18 == UsedSymbols[2] % 18) ||
                                                                                                                                                                                                                                                                                                (Grid[UsedSymbols[3] / 18][UsedSymbols[3] % 18] / 10 == Grid[UsedSymbols[2] / 18][UsedSymbols[2] % 18] / 10) ||
                                                                                                                                                                                                                                                                                                Grid[UsedSymbols[3] / 18][UsedSymbols[3] % 18] % 10 == Grid[UsedSymbols[2] / 18][UsedSymbols[2] % 18] % 10);
                                                                                                                                                                                                                                                                                                if (Bomb.IsIndicatorOff(Indicator.SIG) && !(Bomb.IsPortPresent(Port.RJ45)) && Bomb.GetBatteryCount() == 0) {
                                                                                                                                                                                                                                                                                                   Debug.LogFormat("[Telepathy #{0}] There is an unlit sig, no batteries, and no RJ-45 port. Select any blue lightning bolt.", moduleId);
                                                                                                                                                                                                                                                                                                   RandomAss.Shuffle();
                                                                                                                                                                                                                                                                                                   UsedSymbols[RandomAss[0]] = Unicorn[UnityEngine.Random.Range(0, Unicorn.Length)];
                                                                                                                                                                                                                                                                                                   UsedSymbols[RandomAss[1]] = 320;
                                                                                                                                                                                                                                                                                                   UsedSymbols[RandomAss[2]] = 183;
                                                                                                                                                                                                                                                                                                   UsedSymbols[RandomAss[3]] = 19;
                                                                                                                                                                                                                                                                                                   for (int i = 0; i < 4; i++) {
                                                                                                                                                                                                                                                                                                      Coordinates[RandomAss[i]].text = Alphabet[UsedSymbols[i] / 18] + (UsedSymbols[i] % 18 + 1).ToString();
                                                                                                                                                                                                                                                                                                   }
                                                                                                                                                                                                                                                                                                   return;
                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                for (int i = 0; i < 24; i++) {
                                                                                                                                                                                                                                                                                                   int Temp = (UsedSymbols[int.Parse(Help[i][0].ToString())] / 18) * 18 + (UsedSymbols[int.Parse(Help[i][1].ToString())] % 18);
                                                                                                                                                                                                                                                                                                   if (Grid[UsedSymbols[int.Parse(Help[i][2].ToString())] / 18][UsedSymbols[int.Parse(Help[i][2].ToString())] % 18] / 10 == Grid[Temp / 18][Temp % 18] / 10) {
                                                                                                                                                                                                                                                                                                      if (Grid[UsedSymbols[int.Parse(Help[i][3].ToString())] / 18][UsedSymbols[int.Parse(Help[i][3].ToString())] % 18] % 10 == Grid[Temp / 18][Temp % 18] % 10) {
                                                                                                                                                                                                                                                                                                         for (int ShitNugget = 0; ShitNugget < 4; ShitNugget++) {
                                                                                                                                                                                                                                                                                                            Coordinates[ShitNugget].text = Alphabet[UsedSymbols[ShitNugget] / 18] + (UsedSymbols[ShitNugget] % 18 + 1).ToString();
                                                                                                                                                                                                                                                                                                         }
                                                                                                                                                                                                                                                                                                         Debug.LogFormat("[Telepathy #{0}] Shown coordinates are {1}, {2}, {3}, {4}.", moduleId, Coordinates[0].text, Coordinates[1].text, Coordinates[2].text, Coordinates[3].text);
                                                                                                                                                                                                                                                                                                         Debug.LogFormat("[Telepathy #{0}] One possible answer is {1}.", moduleId, Alphabet[Temp / 18] + (Temp % 18 + 1).ToString());
                                                                                                                                                                                                                                                                                                         SolutionForTPAuto = Temp;
                                                                                                                                                                                                                                                                                                         Debug.LogFormat("[Telepathy #{0}] A puzzle was made in {1} attempts.", moduleId, Timwi + 1);
                                                                                                                                                                                                                                                                                                         return;
                                                                                                                                                                                                                                                                                                      }
                                                                                                                                                                                                                                                                                                   }
                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                for (int i = 0; i < 4; i++)
                                                                                                                                                                                                                                                                                                   UsedSymbols[i] = 0;
                                                                                                                                                                                                                                                                                                Timwi++;
                                                                                                                                                                                                                                                                                                if (Timwi == 2000) {
                                                                                                                                                                                                                                                                                                   GetComponent<KMBombModule>().HandlePass();
                                                                                                                                                                                                                                                                                                   Debug.LogFormat("[Telepathy #{0}] No valid puzzle was generated within 2000 attempts, automatically solving.", moduleId);
                                                                                                                                                                                                                                                                                                   return;
                                                                                                                                                                                                                                                                                                }
                                                                                                                                                                                                                                                                                                goto Restart;
                                                                                                                                                                                                                                                                                             }


   void SquareHighlight (KMSelectable Submit) {
      for (int i = 0; i < 4; i++) {
         if (Submit == Submits[i]) {
            Coordinates[i].color = new Color32(0, 255, 0, 255);
         }
      }
   }

   void SquareHighlightGone (KMSelectable Submit) {
      for (int i = 0; i < 4; i++) {
         if (Submit == Submits[i]) {
            Coordinates[i].color = new Color32(255, 255, 255, 255);
         }
      }
   }

   void MovePress (KMSelectable Moon) {
      Audio.PlaySoundAtTransform("tick", Moon.transform);
      if (Moon == Move[0]) {
         Index--;
         if (Index < 0) {
            Index += 9;
         }
      }
      else if (Moon == Move[1]) {
         Index++;
         Index %= 9;
      }
      else if (Moon == Move[2]) {
         ColorIndex++;
         ColorIndex %= 9;
      }
      else {
         ColorIndex--;
         if (ColorIndex < 0) {
            ColorIndex += 9;
         }
      }
      TheSymbols.GetComponent<SpriteRenderer>().sprite = TheSymbolsButNotRenderer[Index];
      TheSymbols.GetComponent<SpriteRenderer>().color = new Color32(Colors[ColorIndex][0], Colors[ColorIndex][1], Colors[ColorIndex][2], 255);
      if (ColorIndex == 0) {
         Republicans[0].material.color = new Color32(Colors[ColorIndex + 1][0], Colors[ColorIndex + 1][1], Colors[ColorIndex + 1][2], 255);
         Republicans[1].material.color = new Color32(Colors[8][0], Colors[8][1], Colors[8][2], 255);
      }
      else {
         Republicans[0].material.color = new Color32(Colors[(ColorIndex + 1) % 9][0], Colors[(ColorIndex + 1) % 9][1], Colors[(ColorIndex + 1) % 9][2], 255);
         Republicans[1].material.color = new Color32(Colors[ColorIndex - 1][0], Colors[ColorIndex - 1][1], Colors[ColorIndex - 1][2], 255);
      }
   }

   void SubmitPress (KMSelectable Submit) {
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, Submit.transform);
      for (int i = 0; i < 4; i++)
         if (Submit == Submits[i]) {
            if (Bomb.IsIndicatorOff(Indicator.SIG) && !Bomb.IsPortPresent(Port.RJ45) && Bomb.GetBatteryCount() == 0 && (ColorIndex + 1) * 10 + Index + 1 == 11) {
               if (Coordinates[i].text.Any(Penis => "A1E13M15O9".Contains(Penis))) {
                  GetComponent<KMBombModule>().HandlePass();
                  moduleSolved = true;
               }
               else {
                  GetComponent<KMBombModule>().HandleStrike();
               }
            }
            else if (Bomb.GetSerialNumberNumbers().ToArray()[0] % 2 == 0) {
               if (AnswerCheckerColor(i, (ColorIndex + 1) * 10 + Index + 1)) {
                  GetComponent<KMBombModule>().HandlePass();
                  moduleSolved = true;
               }
               else {
                  GetComponent<KMBombModule>().HandleStrike();
               }
            }
            else if (Bomb.GetSerialNumberNumbers().ToArray()[0] % 2 == 1) {
               if (AnswerCheckerShape(i, (ColorIndex + 1) * 10 + Index + 1)) {
                  GetComponent<KMBombModule>().HandlePass();
                  moduleSolved = true;
               }
               else {
                  GetComponent<KMBombModule>().HandleStrike();
               }
            }
         }
   }

   bool AnswerCheckerColor (int i, int Answer) {
      if (Grid[UsedSymbols[i] / 18][UsedSymbols[i] % 18] / 10 != Answer / 10) {
         return false;
      }
      for (int j = 0; j < 24; j++) {
         Weed = Help[j];
         Crack = "";
         for (int Piss = 0; Piss < 4; Piss++) {
            if (Weed[Piss].ToString() != i.ToString()) {
               Crack += Weed[Piss].ToString();
            }
         }
         if (Grid[UsedSymbols[int.Parse(Crack[0].ToString())] / 18][UsedSymbols[int.Parse(Crack[1].ToString())] % 18] / 10 == Answer / 10 && Grid[UsedSymbols[int.Parse(Crack[2].ToString())] / 18][UsedSymbols[int.Parse(Crack[2].ToString())] % 18] % 10 == Answer % 10) {
            Debug.LogFormat("[Telepathy #{0}] Submitting a {1} {2} is correct.", moduleId, ColorsForLogging[Answer / 10 - 1], SymbolsForLogging[Answer % 10 - 1]);
            return true;
         }
      }
      if (!Autosolving) {
         Debug.LogFormat("[Telepathy #{0}] No valid solution could be found with {1} {2} (at least with the button you pressed). The computer strikes you. With a bat.", moduleId, ColorsForLogging[Answer % 10 - 1], SymbolsForLogging[Answer / 10 - 1]);
      }
      return false;
   }

   bool AnswerCheckerShape (int i, int Answer) {
      if (Grid[UsedSymbols[i] / 18][UsedSymbols[i] % 18] % 10 != Answer % 10) {
         return false;
      }
      for (int j = 0; j < 24; j++) {
         Weed = Help[j];
         Crack = "";
         for (int Piss = 0; Piss < 4; Piss++) {
            if (Weed[Piss].ToString() != i.ToString()) {
               Crack += Weed[Piss].ToString();
            }
         }
         if (Grid[UsedSymbols[int.Parse(Crack[0].ToString())] / 18][UsedSymbols[int.Parse(Crack[1].ToString())] % 18] % 10 == Answer % 10 && Grid[UsedSymbols[int.Parse(Crack[2].ToString())] / 18][UsedSymbols[int.Parse(Crack[2].ToString())] % 18] / 10 == Answer / 10) {
            Debug.LogFormat("[Telepathy #{0}] Submitting a {1} {2} is correct.", moduleId, ColorsForLogging[Answer / 10 - 1], SymbolsForLogging[Answer % 10 - 1]);
            return true;
         }
      }
      if (!Autosolving) {
         Debug.LogFormat("[Telepathy #{0}] No valid solution could be found with {1} {2} (at least with the button you pressed). The computer strikes you. With a bat.", moduleId, ColorsForLogging[Answer % 10 - 1], SymbolsForLogging[Answer / 10 - 1]);
      }
      return false;
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} (color) (shape) to select that shape, and TL/TR/BL/BR to press that button. Acceptable options blue, pink, green, brown, yellow, purple, red, orange, grey/white, bolt, die, gem, hand, moon, eye, star, heart, sun, plus some other secret ones.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      Command = Command.Trim().ToUpper();
      string[] Parameters = Command.Split(' ');
      yield return null;
      if (Parameters.Length > 2) {
         yield return "sendtochaterror Speak bitch, I can't read your mind!";
         yield break;
      }
      else if (Parameters.Length == 1) {
         switch (Parameters[0]) {
            case "TL":
               Submits[0].OnInteract();
               break;
            case "TR":
               Submits[1].OnInteract();
               break;
            case "BL":
               Submits[2].OnInteract();
               break;
            case "BR":
               Submits[3].OnInteract();
               break;
            default:
               yield return "sendtochaterror Speak bitch, I can't read your mind!";
               yield break;
         }
      }
      else {
         #region Colors
         if (Parameters[0] == "BLUE")
            while (ColorIndex != 0) {
               Move[2].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[0] == "PINK" || Parameters[0] == "MAGENTA")
            while (ColorIndex != 1) {
               Move[2].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[0] == "GREEN")
            while (ColorIndex != 2) {
               Move[2].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[0] == "BROWN")
            while (ColorIndex != 3) {
               Move[2].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[0] == "YELLOW")
            while (ColorIndex != 4) {
               Move[2].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[0] == "PURPLE")
            while (ColorIndex != 5) {
               Move[2].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[0] == "RED" || Parameters[0] == "VIBRATOR_PINK")
            while (ColorIndex != 6) {
               Move[2].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[0] == "ORANGE")
            while (ColorIndex != 7) {
               Move[2].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[0] == "GREY" || Parameters[0] == "WHITE")
            while (ColorIndex != 8) {
               Move[2].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else {
            yield return "sendtochaterror Speak bitch, I can't read your mind!";
            yield break;
         }
         #endregion
         if (Parameters[1] == "BOLT" || Parameters[1] == "LIGHTNING_BOLT" || Parameters[1] == "LIGHTNING")
            while (Index != 0) {
               Move[1].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[1] == "DIE" || Parameters[1] == "DICE")
            while (Index != 1) {
               Move[1].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[1] == "GEM" || Parameters[1] == "JEWEL" || Parameters[1] == "ASS_PLUG" || Parameters[1] == "BUTT_PLUG")
            while (Index != 2) {
               Move[1].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[1] == "HAND")
            while (Index != 3) {
               Move[1].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[1] == "MOON")
            while (Index != 4) {
               Move[1].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[1] == "EYE")
            while (Index != 5) {
               Move[1].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[1] == "STAR")
            while (Index != 6) {
               Move[1].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[1] == "HEART")
            while (Index != 7) {
               Move[1].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else if (Parameters[1] == "SUN")
            while (Index != 8) {
               Move[1].OnInteract();
               yield return new WaitForSecondsRealtime(.1f);
            }
         else {
            yield return "sendtochaterror Speak bitch, I can't read your mind!";
            yield break;
         }
      }
   }

   IEnumerator TwitchHandleForcedSolve () {
      Autosolving = true;
      for (int i = 0; i < 4; i++) {
         if (Bomb.IsIndicatorOff(Indicator.SIG) && !Bomb.IsPortPresent(Port.RJ45) && Bomb.GetBatteryCount() == 0 && (ColorIndex + 1) * 10 + Index + 1 == 11) {
            if (Coordinates[i].text.Any(Penis => "A1E13M15O9".Contains(Penis))) {
               Submits[i].OnInteract();
            }
         }
      }

      while (Index + 1 != Grid[SolutionForTPAuto / 18][SolutionForTPAuto % 18] % 10) {
         Move[1].OnInteract();
         Debug.Log(Index);
         yield return new WaitForSeconds(.1f);
      }
      while (ColorIndex + 1 != Grid[SolutionForTPAuto / 18][SolutionForTPAuto % 18] / 10) {
         Move[2].OnInteract();
         Debug.Log(ColorIndex);
         yield return new WaitForSeconds(.1f);
      }
      for (int i = 0; i < 4; i++) {
         if (Bomb.GetSerialNumberNumbers().ToArray()[0] % 2 == 0) {
            if (AnswerCheckerColor(i, (ColorIndex + 1) * 10 + Index + 1)) {
               Submits[i].OnInteract();
            }
         }
         else {
            if (AnswerCheckerShape(i, (ColorIndex + 1) * 10 + Index + 1)) {
               Submits[i].OnInteract();
            }
         }
      }
      yield return null;
   }
}
