using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public interface IPhysicManager
{
	IReadOnlyReactiveProperty<bool> HasMovingRBs { get; }
	UniTask WaitForStop(CancellationToken token);
	void RegisterRigidBody(Rigidbody2D rb);
	void ClearAllRigidBodies();
	void ToggleSimulation(bool isActive);
}

public class PhysicManager : IPhysicManager
{
	private ReactiveProperty<bool> _hasMovingRBs = new(false);
	private readonly List<Rigidbody2D> _allRigidBodies = new();

	public IReadOnlyReactiveProperty<bool> HasMovingRBs => _hasMovingRBs;

	public void RegisterRigidBody(Rigidbody2D rb)
	{
		if (rb != null && rb.bodyType is RigidbodyType2D.Dynamic)
		{
			_allRigidBodies.Add(rb);
			rb.simulated = true;
		}
		else
		{
			Debug.LogError("Attempted to register a null Rigidbody2D.");
		}
	}

	public void UnregisterRigidBody(Rigidbody2D rb)
	{
		if (_allRigidBodies.Contains(rb))
		{
			rb.simulated = false;
			_allRigidBodies.Remove(rb);
		}
		else
		{
			Debug.LogError("Attempted to unregister a Rigidbody2D that was not registered.");
		}
	}

	public void ClearAllRigidBodies()
	{
		foreach (var rb in _allRigidBodies)
		{
			if (rb != null)
			{
				rb.simulated = false;
			}
		}
		_allRigidBodies.Clear();
	}

	public void ToggleSimulation(bool isActive)
	{
		Debug.Log($"{nameof(PhysicManager)}: {nameof(ToggleSimulation)}: {isActive}");
		Physics2D.simulationMode = isActive ? SimulationMode2D.FixedUpdate : SimulationMode2D.Script;
		_hasMovingRBs.Value = isActive;
	}

	public async UniTask WaitForStop(CancellationToken token)
	{
		try
		{
			await UniTask.WaitWhile(() =>
			{
				foreach (var rb in _allRigidBodies)
				{
					if (rb != null && rb.IsAwake() && rb.linearVelocity.magnitude >= 0.1f)
					{
						return true;
					}
				}
				return false;
			}, cancellationToken: token);
		}
		catch
		{
			Debug.LogError("WaitForStop was cancelled.");
		}
		ToggleSimulation(false);
	}
}
