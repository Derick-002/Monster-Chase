using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarImage : MonoBehaviour
{
    [Header("Radar Settings")]
    public Sprite[] monsterSprites;
    public Image radarImagePrefab;     // used for monsters
    public RectTransform radarBar;

    private Dictionary<Transform, Image> radarMap = new Dictionary<Transform, Image>();
    private Dictionary<Transform, (Transform left, Transform right)> monsterBounds = new Dictionary<Transform, (Transform, Transform)>();

    // Define radar world bounds
    public float radarWorldMinX = 0f;
    public float radarWorldMaxX = 21800f;
    public float radarWorldMinY = 0f;
    public float radarWorldMaxY = 21800f;

    private void Awake()
    {
        if (radarImagePrefab != null && radarImagePrefab.gameObject.scene.IsValid())
        {
            // This is a scene instance; hide it
            radarImagePrefab.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Create a radar icon for a newly spawned monster
    /// </summary>
    public void CreateRadarIcon(Transform monster, int spriteIndex, Transform leftPos, Transform rightPos)
    {
        if (spriteIndex < 0 || spriteIndex >= monsterSprites.Length)
        {
            Debug.LogError("Invalid sprite index for radar icon");
            return;
        }

        Image newRadarImage = Instantiate(radarImagePrefab, radarBar);
        newRadarImage.rectTransform.localScale = Vector3.one;

        newRadarImage.sprite = monsterSprites[spriteIndex];

        radarMap[monster] = newRadarImage;
        monsterBounds[monster] = (leftPos, rightPos);

        // Immediately map position
        UpdateSingleMonsterIcon(monster, newRadarImage);
    }

    private void Update()
    {
        // Update all monsters
        List<Transform> dead = new List<Transform>();
        foreach (var kvp in radarMap)
        {
            Transform monster = kvp.Key;
            Image radarIcon = kvp.Value;

            if (monster == null)
            {
                dead.Add(monster);
                Destroy(radarIcon.gameObject);
                continue;
            }

            UpdateSingleMonsterIcon(monster, radarIcon);
        }

        foreach (Transform d in dead)
        {
            radarMap.Remove(d);
            monsterBounds.Remove(d);
        }
    }

    private void UpdateSingleMonsterIcon(Transform monster, Image radarIcon)
    {
        var bounds = monsterBounds[monster];
        float minX = bounds.left.position.x;
        float maxX = bounds.right.position.x;

        // Clamp world position
        float clampedX = Mathf.Clamp(monster.position.x, minX, maxX);

        // Normalize
        float normalizedX = (clampedX - minX) / (maxX - minX);

        // Map to radar bar coordinates using pivot
        float anchoredX = normalizedX * radarBar.rect.width - radarBar.rect.width * radarBar.pivot.x;
        float anchoredY = 0f; // monsters move horizontally only

        radarIcon.rectTransform.anchoredPosition = new Vector2(anchoredX, anchoredY);
    }

} // class