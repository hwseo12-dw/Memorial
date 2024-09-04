using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using TMPro;

public class FurniturePlacer : MonoBehaviour
{
    public Button placeFurnitureButton;
    public Button deleteFurnitureButton; // ���� ��ư
    public Button rotateButton; // ȸ�� ��ư (������� ����)
    public GameObject furniturePopupPanel;
    public Button furniturePopupCloseButton;
    public Transform furnitureButtonContainer;
    public Button furnitureButtonPrefab;
    public List<GameObject> furniturePrefabs;
    public Tilemap floorTilemap;
    public Tilemap furnitureTilemap;
    public GameObject placerNotification;

    public static string saveFurniture;

    public GameObject currentFurniture;
    private FurnitureData currentFurnitureData;
    private bool isDeleteMode = false; // ���� ��� Ȱ��ȭ ����
    private bool isRotated = false; // ȸ�� ���� ����

    void Start()
    {
        placeFurnitureButton.onClick.AddListener(OpenFurniturePopup);
        furniturePopupCloseButton.onClick.AddListener(CloseFurniturePopup);
        deleteFurnitureButton.onClick.AddListener(ToggleDeleteMode); // ���� ��ư Ŭ�� �� ���� ��� ���
        rotateButton.onClick.AddListener(ToggleRotate); // ȸ�� ��ư Ŭ�� �� ȸ�� ��� ���� (���� ������� ����)

        furniturePopupPanel.SetActive(false);
        CreateFurnitureButtons();
        placerNotification.SetActive(false);

        SetRotateButtonState(false); // �ʱ� ���¿��� ȸ�� ��ư ��Ȱ��ȭ
    }

    // ���� ��� ��� �Լ�
    void ToggleDeleteMode()
    {
        isDeleteMode = !isDeleteMode; // ���� ��� Ȱ��ȭ/��Ȱ��ȭ ��ȯ
        placerNotification.SetActive(isDeleteMode); // ���� ��� Ȱ��ȭ �� �˸� ǥ��

        if (isDeleteMode)
        {
            SetRotateButtonState(false); // ���� ��尡 Ȱ��ȭ�Ǹ� ȸ�� ��ư ��Ȱ��ȭ
        }
        else
        {
            SetRotateButtonState(currentFurniture != null); // ������ ���õ� ��� ȸ�� ��ư Ȱ��ȭ
        }
    }

    // ȸ�� ����� ����ϴ� �Լ� (���� ������� ����)
    void ToggleRotate()
    {
        if (currentFurniture != null)
        {
            // ������ z���� �������� 90�� ȸ��
            currentFurniture.transform.Rotate(0, 0, 90);
            isRotated = !isRotated; // ȸ�� ���� ���
        }
    }

    // ȸ�� ��ư�� Ȱ��ȭ ���¸� �����ϴ� �Լ�
    void SetRotateButtonState(bool isActive)
    {
        rotateButton.interactable = isActive;
    }

    void Update()
    {
        // ���� ��尡 Ȱ��ȭ�� ���¿��� ���콺 ���� ��ư Ŭ�� �� ���� ����
        if (isDeleteMode && Input.GetMouseButtonDown(0))
        {
            // Ŭ���� ��ġ�� �ִ� ������ Raycast�� ����
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject.CompareTag("Furniture"))
            {
                Vector3Int cellPosition = furnitureTilemap.WorldToCell(hit.collider.transform.position);
                DeleteFurniture(hit.collider.gameObject, cellPosition); // ������ ���� ����
            }
        }
        else if (currentFurniture != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = floorTilemap.WorldToCell(mouseWorldPos);
            Vector3 cellCenterWorld = floorTilemap.GetCellCenterWorld(cellPosition);
            currentFurniture.transform.position = new Vector3(cellCenterWorld.x, cellCenterWorld.y, 0);
            Collider2D collider = currentFurniture.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }
            furniturePopupPanel.SetActive(false);
            placerNotification.SetActive(true);

            bool canPlace = CanPlaceFurniture(cellPosition);
            currentFurniture.GetComponent<SpriteRenderer>().color = canPlace ? Color.white : Color.red;

            if (Input.GetMouseButtonDown(0) && canPlace) // ���� ��ư���� ���� ��ġ
            {
                PlaceFurniture(cellPosition);
                if (collider != null)
                {
                    collider.enabled = true;
                }
                furniturePopupPanel.SetActive(true);
                placerNotification.SetActive(false);
            }
            else if (Input.GetMouseButtonDown(1)) // ������ ��ư���� ���� ȸ��
            {
                ToggleRotate(); // ������ Ŭ�� �� ȸ�� ��� ����
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isDeleteMode)
            {
                ToggleDeleteMode(); // ESC ��ư�� ������ ���� ��� ����
            }
            else
            {
                furniturePopupPanel.SetActive(false);
                placerNotification.SetActive(false);
                DeleteFurniture(currentFurniture, Vector3Int.zero);
            }
        }
    }

    // ���� ���� �Լ�
    public void DeleteFurniture(GameObject furniture, Vector3Int cellPosition)
    {
        if (furniture != null)
        {
            Destroy(furniture); // ���� ������Ʈ �ı�
        }
    }

    void OpenFurniturePopup()
    {
        furniturePopupPanel.SetActive(true);
    }

    void CloseFurniturePopup()
    {
        furniturePopupPanel.SetActive(false);
        placerNotification.SetActive(false);
        DeleteFurniture(currentFurniture, Vector3Int.zero);
    }

    void CreateFurnitureButtons()
    {
        foreach (GameObject furniturePrefab in furniturePrefabs)
        {
            Button newButton = Instantiate(furnitureButtonPrefab, furnitureButtonContainer);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = furniturePrefab.name;
            newButton.onClick.AddListener(() => SelectFurniture(furniturePrefab));
        }
    }

    // ���õ� ������ �����ϴ� �Լ�
    void SelectFurniture(GameObject furniturePrefab)
    {
        if (currentFurniture != null)
        {
            Destroy(currentFurniture);
        }

        currentFurniture = Instantiate(furniturePrefab);
        currentFurnitureData = currentFurniture.GetComponent<FurnitureData>();

        Collider2D collider = currentFurniture.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }

        saveFurniture = furniturePrefab.name;
        SetRotateButtonState(true); // ������ ���õǸ� ȸ�� ��ư Ȱ��ȭ
        Debug.Log("Selected Furniture: " + currentFurniture.name);
    }

    bool CanPlaceFurniture(Vector3Int cellPosition)
    {
        if (currentFurnitureData == null) return false;

        int width = currentFurnitureData.size.x;
        int height = currentFurnitureData.size.y;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int checkPosition = cellPosition + new Vector3Int(x, y, 0);
                if (furnitureTilemap.GetTile(checkPosition) != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    void PlaceFurniture(Vector3Int cellPosition)
    {
        if (currentFurnitureData == null) return;

        int width = currentFurnitureData.size.x;
        int height = currentFurnitureData.size.y;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int placePosition = cellPosition + new Vector3Int(x, y, 0);
                furnitureTilemap.SetTile(placePosition, null);
            }
        }

        currentFurniture.transform.position = furnitureTilemap.GetCellCenterWorld(cellPosition);
        currentFurniture.transform.SetParent(furnitureTilemap.transform);
        currentFurniture.tag = "Furniture";
        currentFurniture = null;
    }
}
