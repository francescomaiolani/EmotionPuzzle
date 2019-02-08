using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//tutti i data statici di cui ha bisogno la classe avatar
//in questo moto si evitano parecchi problemi legati ad eventi e altre cose da utilizzare se questi dati si
//tengono integrati nella classe avatar non statici
public static class AvatarData
{
    public static string[] maleHairNames = new string[] { "Corti", "Medi", "Ricci", "Pelato", "Ciuffo" };
    public static string[] femaleHairNames = new string[] { "Corti", "Medi", "RicciCorti", "Ricci", "Lunghi" };
    public static string[] skinColorNames = new string[] { "PaleWhite", "White", "Olive", "Brown", "Dark" };
    public static string[] eyesColorNames = new string[] { "LightBlue", "Green", "Brown", "Black" };
    public static string[] hairColorNames = new string[] { "DarkBrown", "LightBrown", "Black", "Blonde", "Red" };

    //dictionary con tutti i colori disponibili da customizzare, catalogati per parte del corpo
    public static Dictionary<string, Color32> skinColorDictionary = new Dictionary<string, Color32>
    { { skinColorNames[0], new Color32(255, 208, 160, 255) },
        { skinColorNames[1], new Color32(250, 191, 133, 255) },
        { skinColorNames[2], new Color32(239, 192, 107, 255) },
        { skinColorNames[3], new Color32(176, 139, 77, 255) },
        { skinColorNames[4], new Color32(118, 100, 67, 255) }
    };

    public static Dictionary<string, Color32> eyesColorDictionary = new Dictionary<string, Color32>
    { { eyesColorNames[0], new Color32(115, 183, 167, 255) },
        { eyesColorNames[1], new Color32(136, 183, 65, 255) },
        { eyesColorNames[2], new Color32(94, 69, 45, 255) },
        { eyesColorNames[3], new Color32(60, 60, 60, 255) },

    };
    public static Dictionary<string, Color32> hairColorDictionary = new Dictionary<string, Color32>
    { { hairColorNames[0], new Color32(104, 82, 61, 255) },
        { hairColorNames[1], new Color32(152, 108, 68, 255) },
        { hairColorNames[2], new Color32(61, 52, 48, 255) },
        { hairColorNames[3], new Color32(255, 217, 81, 255) },
        { hairColorNames[4], new Color32(255, 131, 66, 255) }
    };

}
