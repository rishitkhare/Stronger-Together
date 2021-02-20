using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PortraitSpriteArray : ScriptableObject
{
    #region spy portrait fields
    public Sprite spy_default;
    public Sprite spy_bruh01;
    public Sprite spy_bruh02;
    public Sprite spy_blah;
    public Sprite spy_thonk01;
    public Sprite spy_thonk02;
    public Sprite spy_speechless01;
    public Sprite spy_speechless02;
    public Sprite spy_thonk03;
    public Sprite spy_thonk04;
    public Sprite spy_deep;
    public Sprite spy_smile01;
    public Sprite spy_wtf01;
    public Sprite spy_wtf02;
    public Sprite spy_smile02;
    public Sprite spy_smile03;
    public Sprite spy_wtf03;
    public Sprite spy_wtf04;
    public Sprite spy_depress;
    public Sprite spy_heh;
    public Sprite spy_huh01;
    public Sprite spy_huh02;
    #endregion spy portrait fields

    #region prisoner portrait fields
    public Sprite prisoner_default;
    public Sprite prisoner_wait01;
    public Sprite prisoner_wait02;
    public Sprite prisoner_lmao01;
    public Sprite prisoner_die01;
    public Sprite prisoner_heh01;
    public Sprite prisoner_huh01;
    public Sprite prisoner_angry01;
    public Sprite prisoner_angry02;
    public Sprite prisoner_angry03;
    public Sprite prisoner_angry04;
    public Sprite prisoner_angry05;
    public Sprite prisoner_angry06;
    public Sprite prisoner_huh03;
    public Sprite prisoner_huh04;
    public Sprite prisoner_wtf01;
    public Sprite prisoner_wtf02;
    public Sprite prisoner_wtf03;
    public Sprite prisoner_wtf04;
    public Sprite prisoner_wtf05;
    public Sprite prisoner_frick01;
    public Sprite prisoner_frick02;
    public Sprite prisoner_ree01;
    public Sprite prisoner_ree02;
    public Sprite prisoner_ree03;
    public Sprite prisoner_ree04;
    public Sprite prisoner_ree05;
    public Sprite prisoner_ree06;
    public Sprite prisoner_ree07;
    public Sprite prisoner_browtf01;
    public Sprite prisoner_browtf02;
    public Sprite prisoner_browtf03;
    public Sprite prisoner_browtf04;
    public Sprite prisoner_browtf05;
    public Sprite prisoner_hmm;
    public Sprite prisoner_shoot01;
    public Sprite prisoner_grr01;
    public Sprite prisoner_grr02;
    public Sprite prisoner_grr03;
    public Sprite prisoner_grr04;
    public Sprite prisoner_grr05;
    public Sprite prisoner_grr06;
    public Sprite prisoner_shoot02;
    public Sprite prisoner_blank01;
    public Sprite prisoner_blank02;
    public Sprite prisoner_blank03;
    public Sprite prisoner_blank04;
    public Sprite prisoner_blank05;
    public Sprite prisoner_blank06;
    public Sprite prisoner_blank07;
    #endregion prisoner portrait fields

    public Sprite heesPortrait;

    public Sprite GetSprite(string text) {
        switch(text) {
            case ("Spy_default"):
                return spy_default;
            case ("Spy_bruh01"):
                return spy_bruh01;
            case ("Spy_bruh02"):
                return spy_bruh02;
            case ("Spy_blah"):
                return spy_blah;
            case ("Spy_thonk01"):
                return spy_thonk01;
            case ("Spy_thonk02"):
                return spy_thonk02;
            case ("Spy_speechless01"):
                return spy_speechless01;
            case ("Spy_speechless02"):
                return spy_speechless02;
            case ("Spy_thonk03"):
                return spy_thonk03;
            case ("Spy_thonk04"):
                return spy_thonk04;
            case ("Spy_deep"):
                return spy_deep;
            case ("Spy_smile01"):
                return spy_smile01;
            case ("Spy_wtf01"):
                return spy_wtf01;
            case ("Spy_wtf02"):
                return spy_wtf02;
            case ("Spy_smile02"):
                return spy_smile02;
            case ("Spy_smile03"):
                return spy_smile03;
            case ("Spy_wtf03"):
                return spy_wtf03;
            case ("Spy_wtf04"):
                return spy_wtf04;
            case ("Spy_depress"):
                return spy_depress;
            case ("Spy_heh"):
                return spy_heh;
            case ("Spy_huh01"):
                return spy_huh01;
            case ("Spy_huh02"):
                return spy_huh02;
            case ("Prisoner_default"):
                return prisoner_default;
            case ("Prisoner_wait01"):
                return prisoner_wait01;
            case ("Prisoner_wait02"):
                return prisoner_wait02;
            case ("Prisoner_lmao01"):
                return prisoner_lmao01;
            case ("Prisoner_die01"):
                return prisoner_die01;
            case ("Prisoner_heh01"):
                return prisoner_heh01;
            case ("Prisoner_huh01"):
                return prisoner_huh01;
            case ("Prisoner_angry01"):
                return prisoner_angry01;
            case ("Prisoner_angry02"):
                return prisoner_angry02;
            case ("Prisoner_angry03"):
                return prisoner_angry03;
            case ("Prisoner_angry04"):
                return prisoner_angry04;
            case ("Prisoner_angry05"):
                return prisoner_angry05;
            case ("Prisoner_angry06"):
                return prisoner_angry06;
            case ("Prisoner_huh03"):
                return prisoner_huh03;
            case ("Prisoner_huh04"):
                return prisoner_huh04;
            case ("Prisoner_wtf01"):
                return prisoner_wtf01;
            case ("Prisoner_wtf02"):
                return prisoner_wtf02;
            case ("Prisoner_wtf03"):
                return prisoner_wtf03;
            case ("Prisoner_wtf04"):
                return prisoner_wtf04;
            case ("Prisoner_wtf05"):
                return prisoner_wtf05;
            case ("Prisoner_frick01"):
                return prisoner_frick01;
            case ("Prisoner_frick02"):
                return prisoner_frick02;
            case ("Prisoner_ree01"):
                return prisoner_ree01;
            case ("Prisoner_ree02"):
                return prisoner_ree02;
            case ("Prisoner_ree03"):
                return prisoner_ree03;
            case ("Prisoner_ree04"):
                return prisoner_ree04;
            case ("Prisoner_ree05"):
                return prisoner_ree05;
            case ("Prisoner_ree06"):
                return prisoner_ree06;
            case ("Prisoner_ree07"):
                return prisoner_ree07;
            case ("Prisoner_browtf01"):
                return prisoner_browtf01;
            case ("Prisoner_browtf02"):
                return prisoner_browtf02;
            case ("Prisoner_browtf03"):
                return prisoner_browtf03;
            case ("Prisoner_browtf04"):
                return prisoner_browtf04;
            case ("Prisoner_browtf05"):
                return prisoner_browtf05;
            case ("Prisoner_hmm"):
                return prisoner_hmm;
            case ("Prisoner_shoot01"):
                return prisoner_shoot01;
            case ("Prisoner_grr01"):
                return prisoner_grr01;
            case ("Prisoner_grr02"):
                return prisoner_grr02;
            case ("Prisoner_grr03"):
                return prisoner_grr03;
            case ("Prisoner_grr04"):
                return prisoner_grr04;
            case ("Prisoner_grr05"):
                return prisoner_grr05;
            case ("Prisoner_grr06"):
                return prisoner_grr06;
            case ("Prisoner_shoot02"):
                return prisoner_shoot02;
            case ("Prisoner_blank01"):
                return prisoner_blank01;
            case ("Prisoner_blank02"):
                return prisoner_blank02;
            case ("Prisoner_blank03"):
                return prisoner_blank03;
            case ("Prisoner_blank04"):
                return prisoner_blank04;
            case ("Prisoner_blank05"):
                return prisoner_blank05;
            case ("Prisoner_blank06"):
                return prisoner_blank06;
            case ("Prisoner_blank07"):
                return prisoner_blank07;
            case ("Hees_Silhouette"):
                return heesPortrait;
            default:
                return null;
}
    }
}
