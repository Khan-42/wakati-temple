/*
SoundManager.cs
 - by EnderAsh
 
Features:
	- Play looped sequences from one file
	- Jump to the next sequence and loop it
	- Can jump relatively to the next sequence
*/

using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Seq
{
	public int start;
	public int end;
	public bool loop;
}

[RequireComponent(typeof (AudioSource))]
public class SoundManager : MonoBehaviour
{
	public AudioClip sound;
	public Seq[] sequences;
	public int firstSequence;
	
	private float volume = 1.0f;
	private int dir = 1;
	private int pos;
	
	public int Jump(int next, bool relative = false)
	{
		int oldPos = pos;
		pos = next;
		
		audio.timeSamples = sequences[pos].start + (relative ? audio.timeSamples - sequences[oldPos].start : 0);
		
		return oldPos;
	}
	
	public int JumpNext(bool relative = false)
	{
		int oldPos = pos;
		pos++;
		
		audio.timeSamples = sequences[pos].start + (relative ? audio.timeSamples - sequences[oldPos].start : 0);
		
		return oldPos;
	}
	
	public float AudioFade(int direction)
	{
		dir = direction;
		return 0.8f;
	}
	
	void Start()
	{
		audio.clip = sound;
		pos = firstSequence;
		audio.timeSamples = sequences[pos].start;
		
		audio.Play();
	}
	
	void Update()
	{
		if (audio.timeSamples >= sequences[pos].end)
		{
			if (sequences[pos].loop)
			{
				audio.timeSamples = sequences[pos].start;
				audio.Play();
				Debug.Log(pos);
			}
			else
			{
				pos++;
				Debug.Log(pos);
			}
		}
	}
	
	void LateUpdate()
	{
		volume += dir * 0.8f * Time.deltaTime;
		volume = Mathf.Clamp01(volume);
		
		audio.volume = volume;
	}
}