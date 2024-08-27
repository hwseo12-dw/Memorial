using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class FurniturePlacer : MonoBehaviour
{
    public Button placeFurnitureButton;    // ���� ȭ�� '���� ��ġ' ��ư�� ��� ����
    public GameObject furniturePopupPanel;    // ���� ��ġ�� �ʿ��� ��ư��� ������Ʈ�� ��� �˾� �г�
    public Button furniturePopupCloseButton;    // �˾� ȭ���� �ݱ� ���� �ʿ�
    public Transform furnitureButtonContainer;    // ���� ��ư���� ��� ���� �����̳� ����
    public Button furnitureButtonPrefab;    // ���� �̸��� ��� ��ư ������
    public List<GameObject> furniturePrefabs;    // ���ӿ�����Ʈ Ÿ���� ������ ��� ����Ʈ�̴�. ���⿡ ���� �̹������� ������ �ȴ�.
    public Tilemap floorTilemap;    // �ٴ� Ÿ�ϸ� ����
    public Tilemap furnitureTilemap;    // ���� ��ġ�� ���� ������ Ÿ�ϸ�
    public GameObject placerNotification;

    public static string saveFurniture;

    private GameObject currentFurniture;    // SelectFurniture �޼��忡�� ���� �������� ��� ���� ����
    private FurnitureData currentFurnitureData;    // SelectFurniture �޼��忡�� ���� ������ ��� ���� ����


    void Start()
    {
        placeFurnitureButton.onClick.AddListener(OpenFurniturePopup);    // '���� ��ġ' ��ư�� Ŭ���ϸ� �˾� â ���
        furniturePopupCloseButton.onClick.AddListener(CloseFurniturePopup);    // �ݱ� ��ư�� ������ �˾�â�� �ݴ´�.
        furniturePopupPanel.SetActive(false);    // �˾�â�� ������ ������ ���� �� ���̰� '���� ��ġ' ��ư�� ���� ���� ������ �ϹǷ� ó������ false ���� �ش�. �̰� �������� ������ ���� ������ �� �˾�â ����
        CreateFurnitureButtons();    // �̰� ������ �˾�â ��� ���� ��ư���� �� ����
        placerNotification.SetActive(false);
    }

    void OpenFurniturePopup()    // �˾�â�� ���� ���� �޼���
    {
        furniturePopupPanel.SetActive(true);
    }

    void CloseFurniturePopup()    // �˾�â�� �ݱ� ���� �޼���
    {
        furniturePopupPanel.SetActive(false);
        placerNotification.SetActive(false);
        Destroy(currentFurniture);    // �� �ڵ� ������ ���� ��ġ â �ݾƵ� ���콺�� ������ ��������. ������ ��ġ�ؾ��ϴ� ��츦 ����
    }

    void CreateFurnitureButtons()
    {
        // Debug.Log("Creating furniture buttons. Total prefabs: " + furniturePrefabs.Count);    // ������ ��� �����ƴ��� ����ϴ� ����� �ܼ��ε� �ʿ��� �ּ� ó����
        foreach (GameObject furniturePrefab in furniturePrefabs)    // furniturePrefabs ����Ʈ �ȿ� �ִ� �� furniturePrefab�� ��ȸ�Ѵ�.(���ο� ���� ������Ʈ�� ���� ����°� �ƴ�!)
        {
            Button newButton = Instantiate(furnitureButtonPrefab, furnitureButtonContainer);    // �� ��ư�� furnitrueButtonPrefab�� ������� ��������� ���� ������ ��ư�� �θ�� furnitureButtonContainer�� �ȴ�.
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = furniturePrefab.name;
            newButton.onClick.AddListener(() => SelectFurniture(furniturePrefab));    // �˾�â �� ���� ��ư�� Ŭ���ϸ� 'SelectFurniture()' �޼��带 ȣ��
            // Debug.Log("Created button for: " + furniturePrefab.name);    // ���� ������ �̸��� ����ϴ� ����� �ܼ��ε� �ʿ��� �ּ� ó����
        }
    }

    void SelectFurniture(GameObject furniturePrefab)
    {
        if (currentFurniture != null)    // ���� currentFurniture�� null ���� �ƴ϶�� currentFurniture�� ��� �ִ� ���� ���ش�. �� �ڵ尡 ������ ������ �����ϰ� ��ġ���� ���� ���¿��� �ٸ� ������ Ŭ������ �� ������ ������ ������ ������� �ʴ� ������ �����.
        {
            Destroy(currentFurniture);
        }

        // �Ʒ� �ڵ���� ���� if������ ������� �׻� ����ȴ�.
        currentFurniture = Instantiate(furniturePrefab);    // �� ������ ���� ������Ʈ�� ���纻�� �����ϸ�, ��ġ�� ȸ���� ������ �����ϴ�.
        currentFurnitureData = currentFurniture.GetComponent<FurnitureData>();    // �� �ڵ尡 ������ ���� ������ �������� �ʾ� �ƿ� ���� ��ġ�� �ȵȴ�.

        // Collider ��Ȱ��ȭ
        /* Collider2D collider = currentFurniture.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        } */
    }

    void Update()
    {
        if (currentFurniture != null)    // ���� ��ư�� Ŭ���ϸ� currentFurniture�� ���� �Ҵ�ȴ�.
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);    // ���� ���콺 Ŀ���� ��ġ�� ���� ��ǥ��� ��ȯ�Ѵ�. Input.mousePosition�� ȭ�鿡���� ���콺 ��ġ�� �ȼ� ������ ��ȯ�ϸ�, 'Camera.main.ScreenToWorldPoint'�� �� ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ�Ѵ�.
            Vector3Int cellPosition = floorTilemap.WorldToCell(mouseWorldPos);    // ���� ���콺 ��ġ�� �ش��ϴ� Ÿ�ϸ��� �� ��ǥ�� ��Ÿ����. �� ��ǥ�� ������� currentFurniture ������Ʈ�� ��ġ�� ��ġ�� �����ȴ�.
            Vector3 cellCenterWorld = floorTilemap.GetCellCenterWorld(cellPosition);    // Ÿ�ϸ� ���� �߽��� ���� ��ǥ�� ��ȯ�Ѵ�. ���� ��ǥ�� ���� ��ǥ(cellPosition)�� ��Ÿ������, ������Ʈ�� ���� �߽ɿ� ��ġ�ؾ� �ϹǷ�, �̸� ���� GetCellCenterWorld�� ����Ͽ� �� �߽��� ���� ��ǥ�� ��´�.
            currentFurniture.transform.position = new Vector3(cellCenterWorld.x, cellCenterWorld.y, 0);    // currentFurniture�� ��ġ�� �� �߽��� ��ǥ�� �����Ѵ�. ������Ʈ�� 2D ���ӿ��� z��ǥ�� 0�̹Ƿ� x�� y ��ǥ�� �� �߽ɿ� ���߾� ��ġ�ȴ�.
            Collider2D collider = currentFurniture.GetComponent<Collider2D>();    // currentFurniture�� collider �Ӽ��� �����´�. ������ ��ġ�ϱ� ������ collider ���� ���� ��ġ�ϰ� �� �Ŀ� collider ���� �ֱ� ���ؼ���.
            collider.enabled = false;    // ������ ��ġ�ϱ� �� ���콺 �����Ϳ� ������ ���� ���� collider ���� ����. �� �� collider ���� ���������� �̹� ��ġ�Ǿ� �ִ� ������ �浹�� �Ͼ��.
            furniturePopupPanel.SetActive(false);
            placerNotification.SetActive(true);

            // ���� ��ġ ���� ���� Ȯ��
            bool canPlace = CanPlaceFurniture(cellPosition);

            // ���� ���� �������� ��ġ ���� ���� ǥ��
            currentFurniture.GetComponent<SpriteRenderer>().color = canPlace ? Color.white : Color.red;

            if (Input.GetMouseButtonDown(0) && canPlace)    // ���콺 ���� ��ư ������ + calPlace�� true �϶��� ���� ����
            {
                PlaceFurniture(cellPosition);
                collider.enabled = true;    // �����ִ� collider �� Ȱ��ȭ
                furniturePopupPanel.SetActive(true);
                placerNotification.SetActive(false);
            }

            if (Input.GetMouseButton(1))    // �� �ڵ� ������ �Ǽ��� ���� ������ �� ������ ������ ��ġ�ؾ� �Ѵ�. ���� ������ ����ϱ� ���� �ڵ�. ������ ���콺 ��ư�� ���� ����Ѵ�.
            {
                Destroy(currentFurniture);
                placerNotification.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))    // esc�� ������ �˾� â�� ������ �ϴ� �ڵ�
        {
            furniturePopupPanel.SetActive(false);
            placerNotification.SetActive(false);
        }
    }


    bool CanPlaceFurniture(Vector3Int cellPosition)
    {
        if (currentFurnitureData == null) return false;    // currentFurnitureData ���� �ƹ��͵� ���ٴ� ���� ������ �� ������ ���ٴ� ���̹Ƿ� ��ġ ��ü�� �ȵȴ�. �� ���� �����ո��� FurnitureData ��ũ��Ʈ�� ����ġ�ϰ� ������ �������� ��

        // ������ ũ��
        int width = currentFurnitureData.size.x;
        int height = currentFurnitureData.size.y;

        // Vector3Int startPosition = cellPosition - new Vector3Int(width / 2, height / 2, 0);
        // Vector3Int endPosition = cellPosition + new Vector3Int(width / 2, height / 2, 0);

        for (int x = 0; x < currentFurnitureData.size.x; x++)
        {
            for (int y = 0; y < currentFurnitureData.size.y; y++)
            {
                Vector3Int checkPosition = cellPosition + new Vector3Int(x, y, 0);    // ���콺�� ��ġ�� �ʱ� ������ ��� ������ ���� x�� y���� ������Ű�鼭 ��ȿ�� Ÿ������ �˻�
                if (furnitureTilemap.HasTile(checkPosition) || !floorTilemap.HasTile(checkPosition))    // checkPosition�� ������ �ְų� �ٴ��� ������ false�� ��ȯ�Ѵ�. ���� ���� ������ ��ġ�ϰų� �ٴ��� ���� ���� ������ ��ġ�ϸ� �ȵǴϱ�!
                {
                    return false;
                }
            }
        }
        return true;
    }

    void PlaceFurniture(Vector3Int cellPosition)
    {
        for (int x = 0; x < currentFurnitureData.size.x; x++)
        {
            for (int y = 0; y < currentFurnitureData.size.y; y++)
            {
                Vector3Int tilePosition = cellPosition + new Vector3Int(x , y, 0);    // currentFurnitureData.size�� ����� ����ŭ tilePosition�� ũ�� ����
                furnitureTilemap.SetTile(tilePosition, ScriptableObject.CreateInstance<Tile>());    // tilePosition�� ũ�⸸ŭ furnitureTilemap�� �����Ѵ�. �� ���� ������ ��ġ�� �� ����
            }
        }
        
        currentFurniture = null;    // �̰� ������ ���� ��ġ�ص� ���콺���� ������ ������� ����
        currentFurnitureData = null;
    }
}