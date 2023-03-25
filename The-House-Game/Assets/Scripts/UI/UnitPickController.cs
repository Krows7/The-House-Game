using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Settings;
using UnityEngine.UI;
using TMPro;

public class UnitPickController : MonoBehaviour
{
    public Unit Unit { get; set; }

    public int Id { 
        set { 
            GetPreview().Find("BindTitle").GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }

    public void PickUnit()
    {
        if (Unit == null) {
            return;
        }
        var cameraController = GameObject.Find("CameraController").GetComponent<CameraController>();
        Vector3 v = Unit.transform.position * 1;
        v.z = cameraController.getCameraTransform().position.z;
        GameObject.Find("/MovementController").GetComponent<InputController>().ChooseUnit(Unit);
        cameraController.SetCamera(v);
    }

    public void Update()
    {
        var preview = GetPreview();
        preview.localScale = Vector3.one * (Unit == null ? 0 : 1);

        if (Unit == null) return;

        UpdateStradegyIcon();

        var spriteRenderer = preview.GetComponentInChildren<Image>();

        if (spriteRenderer == null) return;

        var unitRenderer = Unit.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = unitRenderer.sprite;
        spriteRenderer.color = unitRenderer.color;

        //This is not right, i know
        if (Unit == InputController.instance.unit && Input.GetKeyDown(KeyCode.F))
        {
            var preventIcon = GetPreview().Find("PreventAutoAttack").GetComponent<Image>();
            preventIcon.enabled = !preventIcon.enabled;
            var afkController = Unit.GetComponent<AFKController>();
            afkController.enabled = !afkController.enabled;
        }

        var progressBar = GetPreview().Find("ProgressBar").transform.GetChild(0).GetChild(0).GetComponent<Image>();
        progressBar.fillAmount = Unit.GetHealth() / Unit.GetMaxHealth();
    }

    private void UpdateStradegyIcon()
    {
        var stradegyRenderer = GetPreview().Find("StradegyRenderer").GetComponent<Image>();
        var movementComponent = Unit.GetComponent<MovementComponent>();

        if (movementComponent.Strategy is SafeMovementStrategy)
        {
            stradegyRenderer.sprite = UnitPickFieldAssetContainer.instance.baseMovementStradegyIcon;
            stradegyRenderer.color = new(0.2490566F, 0.3919672F, 1);
        } else
        {
            stradegyRenderer.sprite = UnitPickFieldAssetContainer.instance.followEnemyStradegyIcon;
            stradegyRenderer.color = new(1, 0.2509804F, 0.2741451F);
        }
    }

    private Transform GetPreview()
    {
        return transform.GetChild(0);
    }
}
