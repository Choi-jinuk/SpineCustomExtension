using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Spine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;

// Spine 사용자 정의 확장 메소드 클래스
public static class SpineCustomExtension
{
    #region SetAnimation
    // MeshRenderer 기반 spine animation 설정
    public static void SetAnimationForUnity(this SkeletonAnimation animation, string animationName, bool loop, int trackIndex = 0)
    {
        if (animation == null || animation.Skeleton == null || animation.state == null)
            return;

        animation.Skeleton.SetToSetupPose();
        animation.state.SetAnimation(trackIndex, animationName, loop);
        animation.state.Apply(animation.Skeleton);
    }
    // CanvasRenderer 기반 spine animation 설정
    public static void SetAnimationForUnity(this SkeletonGraphic graphic, string animationName, bool loop, int trackIndex = 0)
    {
        if (graphic == null)
            return;
        var currentTrack = graphic.AnimationState.GetCurrent(0);
        if (currentTrack != null)
        {
	        if (currentTrack.Animation.Name.Equals(animationName))
		        return;
        }
        
        graphic.Skeleton.SetToSetupPose();
        graphic.AnimationState.SetAnimation(trackIndex, animationName, loop);
        graphic.AnimationState.Apply(graphic.Skeleton);
    }
    public static void AddAnimationForUnity(this SkeletonGraphic graphic, string animationName, bool loop, float delay, int trackIndex = 0)
    {
        if (graphic == null)
            return;

        graphic.AnimationState.AddAnimation(trackIndex, animationName, loop, delay);
    }
    #endregion

    #region SetSkin
    //스킨에 맞춘 전체 코스튬 변경
      
    public static void SetSkin(this SkeletonAnimation animation, string skinName)
    {
        if (animation == null)
            return;

        if (string.IsNullOrEmpty(skinName))
	        skinName = INITIAL_SKIN;
        
        Skin skin = animation.Skeleton.Data.FindSkin(skinName);
        if (skin != null)
        {
	        animation.Skeleton.SetSkin(skin);
        }

        animation.Skeleton.SetSlotsToSetupPose();
        animation.Skeleton.A = 1.0f;
        animation.LateUpdate();
    }
    public static void SetSkin(this SkeletonGraphic animation, string skinName)
    {
        if (animation == null)
            return;

        if (string.IsNullOrEmpty(skinName))
	        skinName = INITIAL_SKIN;
        
        animation.Skeleton.SetSkin(skinName);
        animation.Skeleton.SetSlotsToSetupPose();
        animation.LateUpdate();
    }
    #endregion
    
    #region SetRegion
    //전체 코스튬 변경이 아닌 특정 Region의 Attachment를 변경해 스킨 변경
      
    public static void SetRegion(this SkeletonAnimation skeletonAnimation, string charTID, string weaponName)
    {
	    var weaponTableData = CharacterManager.Instance.GetWeaponTableData(charTID);
	    if (weaponTableData == null)
		    return;

	    if (string.IsNullOrEmpty(weaponName))
		    weaponName = INITIAL_WEAPON_SKIN;
	    
	    if (weaponTableData.GetWeaponData(weaponName, out var weaponData) == false)
		    return;

	    var regionName = weaponData.WEAPON_NAME_1;
	    var targetBone = weaponData.WEAPON_TARGET_BONE_1;
	    if (string.IsNullOrEmpty(regionName)|| string.IsNullOrEmpty(targetBone))
		    return;
	    
	    _SetRegion(skeletonAnimation.Skeleton, skeletonAnimation.skeletonDataAsset, targetBone, regionName, weaponData.X_1, weaponData.Y_1);
    }

    public static void SetRegion(this SkeletonAnimation skeletonAnimation, string charTID, string weaponName, float offsetX, float offsetY)
    {
	    var weaponTableData = CharacterManager.Instance.GetWeaponTableData(charTID);
	    if (weaponTableData == null)
		    return;

	    if (string.IsNullOrEmpty(weaponName))
		    weaponName = INITIAL_WEAPON_SKIN;
	    
	    if (weaponTableData.GetWeaponData(weaponName, out var weaponData) == false)
		    return;

	    var regionName = weaponData.WEAPON_NAME_1;
	    var targetBone = weaponData.WEAPON_TARGET_BONE_1;
	    if (string.IsNullOrEmpty(regionName)|| string.IsNullOrEmpty(targetBone))
		    return;

	    _SetRegion(skeletonAnimation.Skeleton, skeletonAnimation.skeletonDataAsset, targetBone, regionName, offsetX, offsetY);
    }
    public static void SetRegion(this SkeletonGraphic skeletonGraphic, string charTID, string weaponName)
    {	    
	    var weaponTableData = CharacterManager.Instance.GetWeaponTableData(charTID);
	    if (weaponTableData == null)
	    {
		    plDebugManager.LogError($"Null weaponTableData in {charTID}");
		    return;
	    }

	    if (string.IsNullOrEmpty(weaponName))
		    weaponName = INITIAL_WEAPON_SKIN;
	    
	    if (weaponTableData.GetWeaponData(weaponName, out var weaponData) == false)
	    {
		    plDebugManager.LogError($"Null stWeaponData {weaponName} in {charTID}");
		    return;
	    }

	    var regionName = weaponData.WEAPON_NAME_1;
	    var targetBone = weaponData.WEAPON_TARGET_BONE_1;
	    if (string.IsNullOrEmpty(regionName)|| string.IsNullOrEmpty(targetBone))
		    return;

	    _SetRegion(skeletonGraphic.Skeleton, skeletonGraphic.skeletonDataAsset, targetBone, regionName, weaponData.X_1, weaponData.Y_1);
    }
    public static void SetRegion(this SkeletonGraphic skeletonGraphic, string charTID, string weaponName, float offsetX, float offsetY)
    {	    
	    var weaponTableData = CharacterManager.Instance.GetWeaponTableData(charTID);
	    if (weaponTableData == null)
	    {
		    plDebugManager.LogError($"Null weaponTableData in {charTID}");
		    return;
	    }

	    if (string.IsNullOrEmpty(weaponName))
		    weaponName = INITIAL_WEAPON_SKIN;
	    
	    if (weaponTableData.GetWeaponData(weaponName, out var weaponData) == false)
	    {
		    plDebugManager.LogError($"Null stWeaponData {weaponName} in {charTID}");
		    return;
	    }

	    var regionName = weaponData.WEAPON_NAME_1;
	    var targetBone = weaponData.WEAPON_TARGET_BONE_1;
	    if (string.IsNullOrEmpty(regionName)|| string.IsNullOrEmpty(targetBone))
		    return;

	    _SetRegion(skeletonGraphic.Skeleton, skeletonGraphic.skeletonDataAsset, targetBone, regionName, offsetX, offsetY);
    }

    static void _SetRegion(Skeleton skeleton, SkeletonDataAsset skeletonDataAsset, string targetBone, string regionName, float offsetX, float offsetY)
    {
	    if (skeleton == null || skeletonDataAsset == null)
		    return;
	    
	    var arrAsset = skeletonDataAsset.atlasAssets;
	    if (arrAsset == null || arrAsset.Length == 0)
		    return;

	    var atlasAsset = arrAsset[0];
	    if (atlasAsset == null)
		    return;

	    var atlas = atlasAsset.GetAtlas();
	    if (atlas == null)
		    return;

	    float scale = skeletonDataAsset.scale;
	    
	    var boneSlot = skeleton.FindSlot(targetBone);
	    if (boneSlot == null)
	     return;
	    
	    Attachment originalAttachment = boneSlot.Attachment;
	    var region = atlas.FindRegion(regionName);

	    if (region == null) 
	    {
		    boneSlot.Attachment = null;
	    }
	    else if (originalAttachment != null)
	    {
		    region.offsetX = offsetX;
		    region.offsetY = offsetY;
		    boneSlot.Attachment = originalAttachment.GetRemappedClone(region, true, true, scale);
	    } 
	    else 
	    {
		    region.offsetX = offsetX;
		    region.offsetY = offsetY;
		    RegionAttachment newRegionAttachment = region.ToRegionAttachment(region.name, scale);
		    boneSlot.Attachment = newRegionAttachment;
	    }
	    if(boneSlot.Attachment is RegionAttachment regionAttachment)
	    {
		    regionAttachment.UpdateRegion();
	    }
    }
    #endregion

    private const string PET_BUNDLE_NAME = "pet/";
    private const string CHARACTER_BUNDLE_NAME = "character/fantasy_";
    private const string MONSTER_BUNDLE_NAME = "monster/";
    private const string SKELETON_DATA = "_SkeletonData";
    private const string INITIAL_SKIN = "1";
    private const string INITIAL_WEAPON_SKIN = "weapon_0";
    private const string DEFAULT_SKIN = "default";
    private const string DEFAULT_ANIMATION = "Idle";
    
    public static async UniTask LoadCharacterSpine(this SkeletonAnimation skeletonAnimation, string strCharTKey, UniTaskTokenID tokenID)
    {
	    var userData = UserManager.Instance.GetSelf();
	    if (userData == null) return;
	    var characterInstanceManager = userData.GetManager<CharacterInstanceManager>();
	    if (characterInstanceManager == null) return;
	    var charInstanceData = characterInstanceManager.GetCharacter(strCharTKey);
	    if (charInstanceData == null)
	    {
		    await LoadCharacterSpine(skeletonAnimation, strCharTKey, string.Empty, string.Empty, tokenID);
		    return;
	    }

	    var strSkin = charInstanceData.CLOTH_COSTUME_TEMPLATE_DATA?.SKIN ?? INITIAL_SKIN;
	    var strWeaponKey = charInstanceData.WEAPON_COSTUME_TEMPLATE_DATA?.SKIN ?? INITIAL_WEAPON_SKIN;
	    await LoadCharacterSpine(skeletonAnimation, strCharTKey, strSkin, strWeaponKey, tokenID);
    }
    public static async UniTask LoadCharacterSpine(this SkeletonAnimation skeletonAnimation, string strCharTKey, string strSkin, string strWeaponKey, UniTaskTokenID tokenID)
    {
	    if (skeletonAnimation == null)
		    return;
	    var characterTemplate = CharacterManager.Instance.GetTemplate(strCharTKey);
	    if (characterTemplate == null)
		    return;
	    
	    string bundleName = CharacterManager.Instance.IsPet(characterTemplate.CHARACTER_TYPE) ? PET_BUNDLE_NAME : 
						    CharacterManager.Instance.IsMonster(characterTemplate.CHARACTER_TYPE) ? MONSTER_BUNDLE_NAME : 
						    CHARACTER_BUNDLE_NAME;
	    bundleName = StringUtil.Append(bundleName, characterTemplate.SPINE_CODE_NAME.ToLower());
	    string skeletonDataName = StringUtil.Append(characterTemplate.SPINE_CODE_NAME, SKELETON_DATA);
	    
	    strSkin = string.IsNullOrEmpty(strSkin) ? INITIAL_SKIN : strSkin;
	    strWeaponKey = string.IsNullOrEmpty(strWeaponKey) ? INITIAL_WEAPON_SKIN : strWeaponKey;

	    await LoadSpine(skeletonAnimation, bundleName, skeletonDataName, strSkin, tokenID);
		skeletonAnimation.SetRegion(strCharTKey, strWeaponKey);
    }

    public static async UniTask LoadSpine(this SkeletonAnimation skeletonAnimation, string strBundleName, string strSpineCode, string strSkin, UniTaskTokenID tokenID)
    {
	    if (skeletonAnimation == null)
		    return;
	    
	    strSkin = string.IsNullOrEmpty(strSkin) ? INITIAL_SKIN : strSkin;
	    
	    if (strSpineCode.Equals(skeletonAnimation.SkeletonDataAsset ? skeletonAnimation.SkeletonDataAsset.name : string.Empty)
	        && strSkin.Equals(skeletonAnimation.Skeleton?.Skin.Name))
	    {
		    skeletonAnimation.SetAnimationForUnity(DEFAULT_ANIMATION, true);
		    return;
	    }

	    //Cancel Token 을 전달했으면 해당 토큰값을 먼저 취소하고 UniTask 실행
	    UniTaskManager.CancelToken(tokenID);
	    var tokenSource = UniTaskManager.CreateCancelToken(tokenID);
	    
	    SkeletonDataAsset spineAsset = null;
	    try
	    {
		    spineAsset = await AddressableManager.LoadAssetAsync<SkeletonDataAsset>(strSpineCode, true, tokenSource);
	    }
	    catch (OperationCanceledException)
	    {
		    skeletonAnimation.gameObject.SetActive(false);
		    return;
	    }

	    if (!spineAsset)
	    {
		    skeletonAnimation.gameObject.SetActive(false);
		    return;
	    }
	    if (spineAsset.GetSkeletonData(false).FindSkin(strSkin) == null)
	    {
		    strSkin = DEFAULT_SKIN;
		    if (spineAsset.GetSkeletonData(false).FindSkin(strSkin) == null)
		    {
			    skeletonAnimation.gameObject.SetActive(false);
			    return;
		    }
	    }
	    
	    if (spineAsset.Equals(skeletonAnimation.SkeletonDataAsset) == false)
	    {
		    skeletonAnimation.skeletonDataAsset = spineAsset;
		    skeletonAnimation.initialSkinName = strSkin;
		    skeletonAnimation.Initialize(true);
		    skeletonAnimation.SetAnimationForUnity(DEFAULT_ANIMATION, true);
	    }
	    else
	    {
		    skeletonAnimation.SetAnimationForUnity(DEFAULT_ANIMATION, true);
	    }
	    
	    UniTaskManager.DisposeToken(tokenID);
    }
    public static async UniTask LoadCharacterSpine(this SkeletonGraphic skeletonGraphic, CharacterInstanceData charInstanceData, UniTaskTokenID tokenID, string strSkin = "", string strWeaponKey = "")
    {
	    if (charInstanceData == null)
		    return;

	    if(string.IsNullOrEmpty(strSkin))
		    strSkin = charInstanceData.CLOTH_COSTUME_TEMPLATE_DATA?.SKIN ?? INITIAL_SKIN;
	    if (string.IsNullOrEmpty(strWeaponKey))
		    strWeaponKey = charInstanceData.WEAPON_COSTUME_TEMPLATE_DATA?.SKIN ?? INITIAL_WEAPON_SKIN;
	    
	    await LoadCharacterSpine(skeletonGraphic, charInstanceData.TEMPLATE_KEY, strSkin, strWeaponKey, tokenID);
    }
    public static async UniTask LoadCharacterSpine(this SkeletonGraphic skeletonGraphic, string strCharTKey, string strSkin, string strWeaponKey, UniTaskTokenID tokenID)
    {
	    if (skeletonGraphic == null)
		    return;
	    var characterTemplate = CharacterManager.Instance.GetTemplate(strCharTKey);
	    if (characterTemplate == null)
		    return;
	    
	    string bundleName = CharacterManager.Instance.IsPet(characterTemplate.CHARACTER_TYPE) ? PET_BUNDLE_NAME : 
							CharacterManager.Instance.IsMonster(characterTemplate.CHARACTER_TYPE) ? MONSTER_BUNDLE_NAME : 
							CHARACTER_BUNDLE_NAME;
	    bundleName = StringUtil.Append(bundleName, characterTemplate.SPINE_CODE_NAME.ToLower());
	    string skeletonDataName = StringUtil.Append(characterTemplate.SPINE_CODE_NAME, SKELETON_DATA);

	    strSkin = string.IsNullOrEmpty(strSkin) ? INITIAL_SKIN : strSkin;
	    strWeaponKey = string.IsNullOrEmpty(strWeaponKey) ? INITIAL_WEAPON_SKIN : strWeaponKey;
	    
	    await LoadSpine(skeletonGraphic, bundleName, skeletonDataName, strSkin, tokenID);
		skeletonGraphic.SetRegion(strCharTKey, strWeaponKey);
    }

    public static async UniTask LoadSpine(this SkeletonGraphic skeletonGraphic, string strBundleName, string strSpineCode, string strSkin, 
	    UniTaskTokenID tokenID, bool bPreviousCancel = true, bool bReleaseAsset = false)
    {
	    if (skeletonGraphic == null)
		    return;
	    
	    strSkin = string.IsNullOrEmpty(strSkin) ? INITIAL_SKIN : strSkin;
	    if (strSpineCode.Equals(skeletonGraphic.SkeletonDataAsset ? skeletonGraphic.SkeletonDataAsset.name : string.Empty)
	        && strSkin.Equals(skeletonGraphic.Skeleton?.Skin.Name))
	    {
		    skeletonGraphic.SetAnimationForUnity(DEFAULT_ANIMATION, true);
		    return;
	    }

	    //Cancel Token 을 전달했으면 해당 토큰값을 먼저 취소하고 UniTask 실행
	    if(bPreviousCancel)
			UniTaskManager.CancelToken(tokenID);
	    var tokenSource = UniTaskManager.CreateCancelToken(tokenID);
	    
	    SkeletonDataAsset spineAsset = null;
	    try
	    {
		    spineAsset = await AddressableManager.LoadAssetAsync<SkeletonDataAsset>(strSpineCode, bReleaseAsset, tokenSource);
	    }
	    catch (OperationCanceledException)
	    {
		    skeletonGraphic.gameObject.SetActive(false);
		    return;
	    }
	    
	    if (!spineAsset)
	    {
		    skeletonGraphic.gameObject.SetActive(false);
		    return;
	    }
	    if (spineAsset.GetSkeletonData(false).FindSkin(strSkin) == null)
	    {
		    strSkin = DEFAULT_SKIN;
		    if (spineAsset.GetSkeletonData(false).FindSkin(strSkin) == null)
		    {
			    skeletonGraphic.gameObject.SetActive(false);
			    return;
		    }
	    }

	    skeletonGraphic.skeletonDataAsset = spineAsset;
	    skeletonGraphic.allowMultipleCanvasRenderers = true;
	    skeletonGraphic.initialSkinName = strSkin;
	    skeletonGraphic.Clear();
	    skeletonGraphic.Initialize(true);
	    skeletonGraphic.SetAllDirty();
	    skeletonGraphic.UnscaledTime = true;
	    skeletonGraphic.SetAnimationForUnity(DEFAULT_ANIMATION, true);
	    skeletonGraphic.gameObject.SetActive(true);
	    
	    UniTaskManager.DisposeToken(tokenID);
    }
    
    public static async UniTask LoadSpine(this SkeletonGraphic skeletonGraphic, string strBundleName, string strSpineCode, string strSkin, string strAnimation, 
	    UniTaskTokenID tokenID, bool bPreviousCancel = true, bool bReleaseAsset = false, bool bMultipleCanvas = true)
    {
	    if (skeletonGraphic == null)
		    return;
	    
	    strSkin = string.IsNullOrEmpty(strSkin) ? INITIAL_SKIN : strSkin;
	    if (strSpineCode.Equals(skeletonGraphic.SkeletonDataAsset ? skeletonGraphic.SkeletonDataAsset.name : string.Empty)
	        && strSkin.Equals(skeletonGraphic.Skeleton?.Skin.Name))
	    {
		    skeletonGraphic.SetAnimationForUnity(strAnimation, true);
		    return;
	    }
	    
	    //Cancel Token 을 전달했으면 해당 토큰값을 먼저 취소하고 UniTask 실행
	    if(bPreviousCancel)
			UniTaskManager.CancelToken(tokenID);
	    var tokenSource = UniTaskManager.CreateCancelToken(tokenID);
	    
	    SkeletonDataAsset spineAsset = null;
	    try
	    {
		    spineAsset = await AddressableManager.LoadAssetAsync<SkeletonDataAsset>(strSpineCode, bReleaseAsset, tokenSource);
	    }
	    catch (OperationCanceledException)
	    {
		    plDebugManager.Log($"LoadSpine ({strSpineCode}) OperationCanceledException active = false");
		    skeletonGraphic.gameObject.SetActive(false);
		    return;
	    }
	    if (!spineAsset)
	    {
		    plDebugManager.Log($"LoadSpine ({strSpineCode}) !spineAsset active = false");
		    skeletonGraphic.gameObject.SetActive(false);
		    return;
	    }
	    if (spineAsset.GetSkeletonData(false).FindSkin(strSkin) == null)
	    {
		    strSkin = DEFAULT_SKIN;
		    if (spineAsset.GetSkeletonData(false).FindSkin(strSkin) == null)
		    {
			    plDebugManager.Log($"LoadSpine ({strSpineCode}) FindSkin == null active = false");
			    skeletonGraphic.gameObject.SetActive(false);
			    return;
		    }
	    }

	    skeletonGraphic.skeletonDataAsset = spineAsset;
	    skeletonGraphic.allowMultipleCanvasRenderers = bMultipleCanvas;
	    skeletonGraphic.initialSkinName = strSkin;
	    skeletonGraphic.Clear();
	    skeletonGraphic.Initialize(true);
	    skeletonGraphic.SetAllDirty();
	    skeletonGraphic.UnscaledTime = true;
	    skeletonGraphic.SetAnimationForUnity(strAnimation, true);
	    skeletonGraphic.gameObject.SetActive(true);
	    
	    UniTaskManager.DisposeToken(tokenID);
    }
    #region GetAnimationDuration
    public static float GetAnimationDuration(this SkeletonAnimation skeletonAnimation, string animationName)
    {
        if (skeletonAnimation == null)
            return default;

        return skeletonAnimation.Skeleton.Data.FindAnimation(animationName).Duration;
    }
    public static float GetAnimationDuration(this SkeletonGraphic skeletonGraphic, string animationName)
    {
        if (skeletonGraphic == null)
            return default;
        if (skeletonGraphic.SkeletonData == null)
            return default;

        return skeletonGraphic.SkeletonData.FindAnimation(animationName).Duration;
    }
    #endregion

    #region ExistAnimation
    public static bool ExistAnimation(this SkeletonAnimation skeletonAnimation, string animationName)
    {
        if (skeletonAnimation == null)
            return false;

        return (skeletonAnimation.Skeleton.Data.FindAnimation(animationName) != null);
    }
    public static bool ExistAnimation(this SkeletonGraphic skeletonGraphic, string animationName)
    {
        if (skeletonGraphic == null)
            return false;

        return (skeletonGraphic.SkeletonData.FindAnimation(animationName) != null);
    }
    #endregion
}
