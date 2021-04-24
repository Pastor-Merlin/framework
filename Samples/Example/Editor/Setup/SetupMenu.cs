using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class SetupMenu
{
    private const string ControllersPath = "Assets/Shared/Controllers.prefab";

    [MenuItem("MyTools/Setup/Everything", false, 0)]
    public static void SetupEverything()
    {
        //SetupCharacters();
    }

    // [MenuItem("RT/Setup/Characters")]
    // public static void SetupCharacters()
    // {
    //     BoardController bc = (BoardController)AssetDatabase.LoadAssetAtPath(ControllersPath, typeof(BoardController));
    //     int width = bc.prefabs.characterSize.x;
    //     int height = bc.prefabs.characterSize.y;
    //     float colliderScale = 0.8f;

    //     string message = "Setup: " + CharactersPath;

    //     Character character = (Character)AssetDatabase.LoadAssetAtPath(CharacterExcelPath, typeof(Character));

    //     AssetUtility.PrefabsForEach(CharactersPath, (path, prefab) =>
    //     {
    //         CharacterData data = character.dataArray.FirstOrDefault(x => x.Prefab.EndsWith(prefab.name + ".prefab"));

    //         if (data != null)
    //         {
    //             message += "\n\t" + path;

    //             if (prefab.GetComponent<Collider>() == null)
    //                 prefab.AddBoxCollider(width, height);
    //             if (prefab.GetComponent<Rigidbody>() == null)
    //                 prefab.AddComponent<Rigidbody>();
    //             if (prefab.GetComponent<CharacterProperty>() == null)
    //                 prefab.AddComponent<CharacterProperty>();
    //             if (prefab.GetComponent<CharacterControl>() == null)
    //                 prefab.AddComponent<CharacterControl>();
    //             if (prefab.GetComponent<Package>() == null)
    //                 prefab.AddComponent<Package>();
    //             if (prefab.GetComponent<PassiveAbilities>() == null)
    //                 prefab.AddComponent<PassiveAbilities>();
    //             if (prefab.GetComponent<Movement>() == null)
    //                 prefab.AddComponent<Movement>();
    //             if (prefab.GetComponent<Interact>() == null)
    //                 prefab.AddComponent<Interact>();
    //             if (prefab.GetComponent<Attack>() == null)
    //                 prefab.AddComponent<Attack>();
    //             if (prefab.GetComponent<Health>() == null)
    //                 prefab.AddComponent<Health>();
    //             if (prefab.GetComponent<ActiveAbilities>() == null)
    //                 prefab.AddComponent<ActiveAbilities>();
    //             if (prefab.GetComponent<Buffs>() == null)
    //                 prefab.AddComponent<Buffs>();
    //             if (prefab.GetComponent<Properties>() == null)
    //                 prefab.AddComponent<Properties>();

    //             PrefabUtility.SavePrefabAsset(prefab);

    //             prefab.GetComponent<Rigidbody>().isKinematic = true;
    //             prefab.GetComponent<BoxCollider>().size = new Vector3(width * colliderScale, 1, height * colliderScale);

    //             CharacterProperty property = prefab.GetComponent<CharacterProperty>();
    //             property.data = data;
    //             property.childTransform = prefab.transform.GetChild(0);

    //             if (property.childTransform == null)
    //                 Debug.LogError($"{prefab}: ChildTransform not found!", prefab);

    //             AnimatorOverrideController overrideController = prefab.GetComponentInChildren<Animator>().runtimeAnimatorController as AnimatorOverrideController;
    //             List<KeyValuePair<AnimationClip, AnimationClip>> overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>(overrideController.overridesCount);
    //             overrideController.GetOverrides(overrides);
    //             // 获取Clip的长度
    //             foreach (KeyValuePair<AnimationClip, AnimationClip> kv in overrides)
    //             {
    //                 if (kv.Key.name.Equals(CharacterAnimationMoveName))
    //                 {
    //                     message += "\n\t\t" + kv.Value.name + "\t\tlenght: " + kv.Value.length;
    //                     property.moveAnimationDuration = kv.Value.length;
    //                 }
    //                 else if (kv.Key.name.Equals(CharacterAnimationJumpName))
    //                 {
    //                     message += "\n\t\t" + kv.Value.name + "\t\tlenght: " + kv.Value.length;
    //                     property.jumpAnimationDuration = kv.Value.length;
    //                 }
    //                 else if (kv.Key.name.Equals(CharacterAnimationAttackName))
    //                 {
    //                     message += "\n\t\t" + kv.Value.name + "\t\tlenght: " + kv.Value.length;
    //                     property.attackAnimationDuration = kv.Value.length;
    //                 }
    //                 else if (kv.Key.name.Equals(CharacterAnimationDieName))
    //                 {
    //                     message += "\n\t\t" + kv.Value.name + "\t\tlenght: " + kv.Value.length;
    //                     property.dieAnimationDuration = kv.Value.length;
    //                 }
    //                 else if (kv.Key.name.Equals(CharacterAnimationTakeDamageName))
    //                 {
    //                     message += "\n\t\t" + kv.Value.name + "\t\tlenght: " + kv.Value.length;
    //                     property.takeDamageAnimationDuration = kv.Value.length;
    //                 }
    //             }

    //             // 获得State的Speed
    //             AnimatorStateMachine sm = (overrideController.runtimeAnimatorController as AnimatorController).layers[0].stateMachine;
    //             for (int i = 0; i < sm.states.Length; ++i)
    //             {
    //                 AnimatorState state = sm.states[i].state;
    //                 if (state.name.Equals(CharacterStateBase))
    //                 {
    //                     message += "\n\t\t" + state.name + "\t\tspeed: " + state.speed;
    //                     property.moveAnimationDuration /= state.speed;
    //                 }
    //                 else if (state.name.Equals(CharacterStateJump))
    //                 {
    //                     message += "\n\t\t" + state.name + "\t\tspeed: " + state.speed;
    //                     property.jumpAnimationDuration /= state.speed;
    //                 }
    //                 else if (state.name.Equals(CharacterStateAttack))
    //                 {
    //                     message += "\n\t\t" + state.name + "\t\tspeed: " + state.speed;
    //                     property.attackAnimationDuration /= state.speed;
    //                 }
    //                 else if (state.name.Equals(CharacterStateDie))
    //                 {
    //                     message += "\n\t\t" + state.name + "\t\tspeed: " + state.speed;
    //                     property.dieAnimationDuration /= state.speed;
    //                 }
    //                 else if (state.name.Equals(CharacterStateTakeDamage))
    //                 {
    //                     message += "\n\t\t" + state.name + "\t\tspeed: " + state.speed;
    //                     property.takeDamageAnimationDuration /= state.speed;
    //                 }
    //             }

    //             foreach (Transform child in prefab.GetComponentsInChildren<Transform>())
    //             {
    //                 if (child.name == "Weapon_L")
    //                     property.leftWeapon = child;

    //                 if (child.name == "Weapon_R")
    //                     property.rightWeapon = child;

    //                 child.gameObject.layer = Layers.CharacterLayer;
    //                 child.gameObject.tag = Tags.Player;
    //             }

    //             if (property.leftWeapon == null)
    //                 Debug.LogError($"{prefab}: Weapon_L not found!", prefab);
    //             if (property.rightWeapon == null)
    //                 Debug.LogError($"{prefab}: Weapon_R not found!", prefab);

    //             foreach (Transform child in property.leftWeapon.Cast<Transform>().ToList())
    //                 GameObject.DestroyImmediate(child.gameObject, true);
    //             foreach (Transform child in property.rightWeapon.Cast<Transform>().ToList())
    //                 GameObject.DestroyImmediate(child.gameObject, true);

    //             PrefabUtility.SavePrefabAsset(prefab);
    //         }
    //         else
    //         {
    //             Debug.LogWarning($"{path}: Excel data not exist!", prefab);
    //         }
    //     });

    //     Debug.Log(message);
    // }

}
