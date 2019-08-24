﻿using Rewired;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    // SETUP
    [SerializeField]
    private GameObject assignedUIGroup;
    [SerializeField]
    private GameObject controller;
    [SerializeField]
    private Image controllerImage;
    [SerializeField]
    private GameObject keyboard;
    [SerializeField]
    private Image keyboardImage;

    // GAMEPLAYER INFO
    [SerializeField]
    private GameObject gameplayerInfoUIGroup;

    // UNASSIGNED
    [SerializeField]
    private GameObject unassignedUIGroup;

    // WAITING
    [SerializeField]
    private GameObject waitingUIGroup;

    // PLAYING
    [SerializeField]
    private GameObject playingUIGroup;
    [SerializeField]
    private UICombinedFilledImage healthBarImage;
    [SerializeField]
    private TMPro.TextMeshProUGUI healthPercentage;
    [SerializeField]
    private TMPro.TextMeshProUGUI goldText;
    [SerializeField]
    private Animator coinAnimator;
    [SerializeField]
    private TMPro.TextMeshProUGUI killCount;
    [SerializeField]
    private UICombinedFilledImage barFilledImage;
    [SerializeField]
    private Image king;
    [SerializeField]
    private GameObject kingGO;

    // END OF BATTLE
    [SerializeField]
    GameObject endOfBattleGO;
    [SerializeField]
    Image endOfBattleFlag;

    // SELECTING SHIP
    [SerializeField]
    private GameObject selectingShipUIGroup;
    [SerializeField]
    private TMPro.TextMeshProUGUI shipName;
    [SerializeField]
    private UnityEngine.UI.Image shipImage;

    private void Start()
    {
        SetHealthPer(1);
        OnGoldChanged(0);
        OnKillsChanged(0);
    }

    public void ConnectToGamePlayer(GamePlayer gamePlayer)
    {
        gamePlayer.onGoldChanged += OnGoldChanged;
        gamePlayer.onShipChanged += OnShipSelectionChanged;
        gamePlayer.onKillsChanged += OnKillsChanged;
        gamePlayer.onColourChanged += OnColourChanged;
        gamePlayer.OnKingStatusChanged += OnKingStatusChanged;
    }

    public void ConnectToCastleShip(CastleShip castleShip)
    {
        castleShip.DamageableRef.onHpChanged += OnHPChanged;
    }
    public void DisconnectCastleShip(CastleShip castleShip)
    {
        castleShip.DamageableRef.onHpChanged -= OnHPChanged;
    }

    private void SetHealthPer(float healthPer)
    {
        healthBarImage.SetFillAmount(healthPer);
        healthPercentage.text = ((int)(healthPer * 100)).ToString();
    }

    private void OnHPChanged(int currentHealth, int maxHealth, float hpPercentage)
    {
        SetHealthPer(hpPercentage);
    }

    private void OnGoldChanged(int gold)
    {
        goldText.text = gold.ToString();
        coinAnimator.SetTrigger("Get");
    }

    private void OnShipSelectionChanged(CastleShip ship)
    {
        this.shipImage.sprite = ship.Image;
        this.shipName.text = ship.ShipName;
    }

    private void OnKillsChanged(int killCount)
    {
        this.killCount.text = killCount.ToString();
    }

    private void OnColourChanged(Color colour)
    {
        this.shipImage.color = colour;
        this.controllerImage.color = colour;
        this.keyboardImage.color = colour;
        this.barFilledImage.SetColor(colour);
        this.king.color = colour;
        this.endOfBattleFlag.color = colour;
    }

    private void OnKingStatusChanged(bool value)
    {
        this.kingGO.SetActive(value);
    }

    void TurnAllOff()
    {
        this.gameplayerInfoUIGroup.SetActive(false);
        this.assignedUIGroup.SetActive(false);
        this.waitingUIGroup.SetActive(false);
        this.playingUIGroup.SetActive(false);
        this.selectingShipUIGroup.SetActive(false);
        this.unassignedUIGroup.SetActive(false);
        this.endOfBattleGO.SetActive(false);
    }

    public void ChangeToAssigned(PlayerManager.PlayerArgs playerArgs)
    {
        TurnAllOff();
        if (ReInput.players.GetPlayer(playerArgs.PlayerId).controllers.GetLastActiveController().type == ControllerType.Joystick)
        {
            this.controller.SetActive(true);
            this.keyboard.SetActive(false);
        }
        else if (ReInput.players.GetPlayer(playerArgs.PlayerId).controllers.GetLastActiveController().type == ControllerType.Mouse 
            || ReInput.players.GetPlayer(playerArgs.PlayerId).controllers.GetLastActiveController().type == ControllerType.Keyboard)
        {
            this.controller.SetActive(false);
            this.keyboard.SetActive(true);
        }
        this.assignedUIGroup.SetActive(true);
    }

    public void ChangeToUnassigned()
    {
        TurnAllOff();
        this.unassignedUIGroup.SetActive(true);
    }

    public void ChangeToWaiting()
    {
        TurnAllOff();
        this.waitingUIGroup.SetActive(true);
    }

    public void ChangeToPlaying()
    {
        TurnAllOff();
        this.playingUIGroup.SetActive(true);
        this.gameplayerInfoUIGroup.SetActive(true);
    }

    public void ChangeToShipSelection()
    {
        TurnAllOff();
        this.selectingShipUIGroup.SetActive(true);
        this.gameplayerInfoUIGroup.SetActive(true);
    }

    public void ChangeToEndOfBattle()
    {
        TurnAllOff();
        this.endOfBattleGO.SetActive(true);
    }
}