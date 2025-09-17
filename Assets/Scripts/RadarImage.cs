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

    private Transform player;          // player reference
    private Image playerRadarIcon;     // player icon

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
    /// Set the player for the radar
    /// </summary>
    public void SetPlayer(Transform playerTransform, Sprite playerSprite)
    {
        player = playerTransform;

        Debug.Log("The radar width: " + radarBar.rect);

        // Create a new GameObject for the player icon (avoid prefab issues)
        GameObject playerGO = new GameObject("PlayerRadarIcon", typeof(RectTransform), typeof(Image));
        playerGO.transform.SetParent(radarBar, false);

        // Configure RectTransform
        RectTransform rt = playerGO.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(20, 20); // adjust size if needed
        rt.localScale = Vector3.one;
        rt.pivot = new Vector2(0.5f, 0.5f);

        // Configure Image component
        Image img = playerGO.GetComponent<Image>();
        img.sprite = playerSprite;
        img.preserveAspect = true;

        // Assign to field
        playerRadarIcon = img;

        // Immediately map player's world position to radar
        MapPlayerPositionToRadar();
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

        // Update player icon
        MapPlayerPositionToRadar();
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

    private void MapPlayerPositionToRadar()
    {
        if (player == null || playerRadarIcon == null)
            return;

        // Clamp world position
        float clampedX = Mathf.Clamp(player.position.x, radarWorldMinX, radarWorldMaxX);
        float clampedY = Mathf.Clamp(player.position.y, radarWorldMinY, radarWorldMaxY);

        // Normalize
        float normalizedX = (clampedX - radarWorldMinX) / (radarWorldMaxX - radarWorldMinX);
        float normalizedY = (clampedY - radarWorldMinY) / (radarWorldMaxY - radarWorldMinY);

        // Map normalized to radar bar coordinates based on pivot
        float anchoredX = normalizedX * radarBar.rect.width - radarBar.rect.width * radarBar.pivot.x;
        float anchoredY = normalizedY * radarBar.rect.height - radarBar.rect.height * radarBar.pivot.y;

        playerRadarIcon.rectTransform.anchoredPosition = new Vector2(anchoredX, anchoredY);
    }
} // class