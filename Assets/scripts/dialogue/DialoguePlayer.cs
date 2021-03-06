﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePlayer : MonoBehaviour {

	[SerializeField] GameObject dialogueUI;
	[SerializeField] GameObject dialogueVeil;
	[SerializeField] Image characterSprite;
	[SerializeField] Text nameText;
	[SerializeField] Text lineText;
	[SerializeField] List<RawImage> panelImages;
	[SerializeField] bool autoProgress = false;
	[SerializeField] float lingerTime = 2f;

	public static DialoguePlayer instance;

	Dialogue dialogue;
	DialoguePiece currentPiece;
	string currentLine;
	int pieceIndex;
	int lineIndex;
	Coroutine playLineRoutine;
	Coroutine lingerRoutine;
	bool lingering;
	bool playing;
	[HideInInspector] public bool hideDialogAtEndFlag = true;
	[HideInInspector] public bool usingVeil = false;

	void Awake() {
		Debug.Assert(instance == null);
		instance = this;
		dialogueVeil.SetActive(false);
		dialogueUI.SetActive(false);
	}

	void Update() {
		// if (Input.GetMouseButtonDown(0))
		// 	Interrupt();
	}

	public void Interrupt() {
		if (lingering) {
			StopCoroutine(lingerRoutine);
			lingering = false;
			PlayNextLine();
		} else if (playing) {
			StopCoroutine(playLineRoutine);
			playing = false;
			lineText.text = currentLine;
			lingerRoutine = StartCoroutine(LingerRoutine());
		}
	}

	public void PlayDialogue(Dialogue dialogue) {
		dialogueUI.SetActive(true);
		this.dialogue = dialogue;
		pieceIndex = 0;
		lineIndex = 0;
		PlayNextPiece();
	}

	public void PlayMainDialogue(Dialogue dialogue) {
		dialogueVeil.SetActive(true);
		usingVeil = true;
		PlayDialogue(dialogue);
	}

	void PlayNextPiece() {
		if (pieceIndex == dialogue.pieces.Count) {
			FinishDialogue();
			return;
		}
		UpdateUI();
		PlayNextLine();
		pieceIndex++;
	}

	void UpdateUI() {
		currentPiece = dialogue.pieces[pieceIndex];
		characterSprite.sprite = currentPiece.character.sprite;
		nameText.text = currentPiece.character.name;
		nameText.color = currentPiece.character.textColor;
		lineText.color = currentPiece.character.textColor;
		foreach (RawImage rImage in panelImages) {
			rImage.color = currentPiece.character.bgColor;
		}
	}

	void PlayNextLine() {
		if (lineIndex == currentPiece.lines.Count) {
			lineIndex = 0;
			PlayNextPiece();
			return;
		}
		if (playLineRoutine != null)
			StopCoroutine(playLineRoutine);
		currentLine = currentPiece.lines[lineIndex];
		playLineRoutine = StartCoroutine(PlayNextLineRoutine(currentLine));
		lineIndex++;
	}

	IEnumerator PlayNextLineRoutine(string line) {
		playing = true;
		lineText.text = "";
		foreach(char c in line.ToCharArray()) {
			dialogueUI.SetActive(true);
			if( usingVeil )
				dialogueVeil.SetActive(true);
			lineText.text += c;
			yield return new WaitForSeconds(0.03f);
		}
		playing = false;
		lingerRoutine = StartCoroutine(LingerRoutine());
	}

	IEnumerator LingerRoutine() {
		lingering = true;
		if( autoProgress )
			yield return new WaitForSeconds(lingerTime);
		lingering = false;
		yield return null;
		PlayNextLine();
	}

	void FinishDialogue() {
		// Debug.Log("Dialogue Player has finished!");
		dialogue.Finish();
		dialogueVeil.SetActive(false);
		if( hideDialogAtEndFlag)
			dialogueUI.SetActive(false);
	}
}
