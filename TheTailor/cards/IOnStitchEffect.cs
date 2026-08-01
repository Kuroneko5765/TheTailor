using MegaCrit.Sts2.Core.Models;

public interface IOnStitchEffect
{
    public void OnStitch(CardModel card, CardModel stitchedCard);
    public void OnUnstitch(CardModel card);
}