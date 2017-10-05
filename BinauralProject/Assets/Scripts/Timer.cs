using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	[SerializeField]
	private Text _timerView;
	[SerializeField]
	private float _timeCount;
	public float timeCount { 
		get { return _timeCount; }
	}
	private bool _isBorn = false;
	private GameController _gc;
	private PlayerController _pc;
	//private CallBack callBack;
	// Use this for initialization
	void Start () {
		_gc = GetComponent<GameController>();
		_pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_gc.isPlay) {
			_timeCount -= Time.deltaTime;
			_timerView.text = _timeCount.ToString("f2");
			if(!_isBorn && _timeCount <= 30f) {
				_gc.GenerateBoss();
				_isBorn = true;
			}
			if(_timeCount < 0) {
				_timeCount = 0;
				_gc.isPlay = false;
			}
		}
	}
}
