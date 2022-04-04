using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LudumDare50
{
	public class CharacterAnimation : MonoBehaviour
	{
		[SerializeField] private NavMeshAgent _agent;
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private Animator _animator;
		[SerializeField] private string _walkSpeedPropName;
		[SerializeField] private AnimationCurve _walkSpeedOverVelocity = AnimationCurve.Linear(0, 0, 1, 1);

		private int walkSpeedProp;


		private void Awake()
		{
			walkSpeedProp = Animator.StringToHash(_walkSpeedPropName);
		}

		private void Update()
		{
			float velocity = Mathf.Max(_agent.velocity.magnitude, _rigidbody.velocity.magnitude);
			float walkSpeed = _walkSpeedOverVelocity.Evaluate(velocity);

			_animator.SetFloat(walkSpeedProp, walkSpeed);
		}

		private void Reset()
		{
			_animator = GetComponentInChildren<Animator>();
			_agent = GetComponent<NavMeshAgent>();
			_rigidbody = GetComponent<Rigidbody>();
		}
	}
}
