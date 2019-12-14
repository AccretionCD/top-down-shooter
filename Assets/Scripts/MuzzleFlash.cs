using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] Sprite[] muzzleFlashSprites;
    [SerializeField] SpriteRenderer[] spriteRenderers;

    void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        muzzleFlash.SetActive(true);

        int muzzleFlashSpritesIndex = Random.Range(0, muzzleFlashSprites.Length);

        for (int i = 0; i < spriteRenderers.Length; i++)
            spriteRenderers[i].sprite = muzzleFlashSprites[muzzleFlashSpritesIndex];

        Invoke("Deactivate", 0.05f);
    }

    public void Deactivate()
    {
        muzzleFlash.SetActive(false);
    }
}
