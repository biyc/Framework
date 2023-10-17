using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

//will auto register in unity
#if UNITY_5_3_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static private void RegisterBindingAction()
        {
            ILRuntime.Runtime.CLRBinding.CLRBindingUtils.RegisterBindingAction(Initialize);
        }


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            UnityEngine_Vector3_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            UnityEngine_RectTransform_Binding.Register(app);
            UnityEngine_Rect_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);
            UnityEngine_Quaternion_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            DG_Tweening_ShortcutExtensions_Binding.Register(app);
            System_Type_Binding.Register(app);
            UnityEngine_UI_Button_Binding.Register(app);
            UnityEngine_Events_UnityEventBase_Binding.Register(app);
            UnityEngine_Events_UnityEvent_Binding.Register(app);
            UnityEngine_Mathf_Binding.Register(app);
            UnityEngine_UI_Graphic_Binding.Register(app);
            UnityEngine_Color_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            UnityEngine_Animator_Binding.Register(app);
            UnityEngine_RuntimeAnimatorController_Binding.Register(app);
            System_String_Binding.Register(app);
            UnityEngine_AnimationClip_Binding.Register(app);
            System_Action_Binding.Register(app);
            DG_Tweening_DOTween_Binding.Register(app);
            DG_Tweening_Tween_Binding.Register(app);
            UnityEngine_CanvasGroup_Binding.Register(app);
            DG_Tweening_ShortcutExtensions46_Binding.Register(app);
            UnityEngine_Sprite_Binding.Register(app);
            UnityEngine_Texture2D_Binding.Register(app);
            CaptureByRect_Binding.Register(app);
            CaptureByRectV2_Binding.Register(app);
            UnityEngine_Texture_Binding.Register(app);
            System_DateTime_Binding.Register(app);
            System_TimeSpan_Binding.Register(app);
            System_Convert_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            System_Int64_Binding.Register(app);
            System_Int32_Binding.Register(app);
            System_Array_Binding.Register(app);
            UnityEngine_UI_CanvasScaler_Binding.Register(app);
            System_Math_Binding.Register(app);
            UnityEngine_ImageConversion_Binding.Register(app);
            UnityEngine_Application_Binding.Register(app);
            System_IO_File_Binding.Register(app);
            System_Action_1_Texture2D_Binding.Register(app);
            UnityEngine_UI_Image_Binding.Register(app);
            System_Collections_Generic_List_1_String_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding.Register(app);
            CodeStage_AntiCheat_ObscuredTypes_ObscuredLong_Binding.Register(app);
            ETModel_Game_Binding.Register(app);
            ETModel_Hotfix_Binding.Register(app);
            System_Reflection_Assembly_Binding.Register(app);
            System_Linq_Enumerable_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding.Register(app);
            System_Collections_Generic_List_1_Type_Binding_Enumerator_Binding.Register(app);
            System_Reflection_MemberInfo_Binding.Register(app);
            System_IDisposable_Binding.Register(app);
            Blaze_Utility_Tuner_Binding.Register(app);
            System_Text_Encoding_Binding.Register(app);
            System_Char_Binding.Register(app);
            System_Object_Binding.Register(app);
            Blaze_Resource_Res_Binding.Register(app);
            Blaze_Resource_Asset_IUAsset_Binding.Register(app);
            Blaze_Utility_Extend_StringExtension_Binding.Register(app);
            System_Numerics_BigInteger_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Int32_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Array_Binding.Register(app);
            System_Collections_Generic_List_1_Vector3_Binding.Register(app);
            System_Enum_Binding.Register(app);
            System_Collections_Generic_List_1_String_Binding_Enumerator_Binding.Register(app);
            System_Action_1_Sprite_Binding.Register(app);
            Sirenix_Utilities_LinqExtensions_Binding.Register(app);
            Blaze_Utility_Helper_TimeHelper_Binding.Register(app);
            UniRx_Observable_Binding.Register(app);
            UniRx_ObservableExtensions_Binding.Register(app);
            Blaze_Common_VersionInfo_Binding.Register(app);
            Blaze_Utility_Helper_VersionHelper_Binding.Register(app);
            System_Decimal_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Boolean_Binding.Register(app);
            System_Collections_Generic_HashSet_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_ILTypeInstance_Action_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_Int64_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int64_Action_1_Int64_Binding.Register(app);
            System_Action_1_Int64_Binding.Register(app);
            System_Action_1_Boolean_Binding.Register(app);
            System_Action_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_String_Binding.Register(app);
            System_Boolean_Binding.Register(app);
            UnityEngine_Audio_AudioMixer_Binding.Register(app);
            UnityEngine_Audio_AudioMixerSnapshot_Binding.Register(app);
            Blaze_Resource_Common_PrefabObject_Binding.Register(app);
            SoundMixer_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_AudioMixerGroup_Binding.Register(app);
            UnityEngine_AudioSource_Binding.Register(app);
            UnityEngine_PlayerPrefs_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_AudioClip_Binding.Register(app);
            UnityEngine_AudioClip_Binding.Register(app);
            Blaze_Resource_Common_AssetIndex_Binding.Register(app);
            System_Exception_Binding.Register(app);
            DG_Tweening_TweenSettingsExtensions_Binding.Register(app);
            System_Collections_Generic_Queue_1_AudioSource_Binding.Register(app);
            System_Collections_Generic_List_1_AudioSource_Binding.Register(app);
            System_Collections_Generic_List_1_AudioSource_Binding_Enumerator_Binding.Register(app);
            ETModel_Component_Binding.Register(app);
            ETModel_Scene_Binding.Register(app);
            System_NotImplementedException_Binding.Register(app);
            System_Collections_IDictionary_Binding.Register(app);
            System_Threading_Thread_Binding.Register(app);
            UnityEngine_Screen_Binding.Register(app);
            System_Collections_Generic_List_1_PrefabObject_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_GameObject_ILTypeInstance_Binding_ValueCollection_Binding_Enumerator_Binding.Register(app);
            System_Threading_Tasks_Task_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_GameObject_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_GameObject_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_KeyValuePair_2_String_GameObject_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_MethodInfo_Binding.Register(app);
            System_Reflection_FieldInfo_Binding.Register(app);
            System_Collections_IEnumerator_Binding.Register(app);
            UnityEngine_UI_Text_Binding.Register(app);
            System_Reflection_MethodBase_Binding.Register(app);
            UnityEngine_UI_Selectable_Binding.Register(app);
            UnityEngine_Material_Binding.Register(app);
            UnityEngine_LayerMask_Binding.Register(app);
            UnityEngine_Canvas_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncTaskMethodBuilder_1_ILTypeInstance_Binding.Register(app);
            System_Threading_Tasks_Task_1_ILTypeInstance_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_ILTypeInstance_Binding.Register(app);
            System_Threading_Tasks_Task_1_PrefabObject_Binding.Register(app);
            System_Runtime_CompilerServices_TaskAwaiter_1_PrefabObject_Binding.Register(app);
            ETModel_IdGenerater_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_HashSet_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Collections_Generic_List_1_Object_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_List_1_ILTypeInstance_Binding.Register(app);
            ETModel_UnOrderMultiMap_2_Type_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Queue_1_Int64_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding_Enumerator_Binding.Register(app);
            System_Activator_Binding.Register(app);
            ETModel_EventAttribute_Binding.Register(app);
            ETModel_EventProxy_Binding.Register(app);
            ETModel_EventSystem_Binding.Register(app);
            LitJson_JsonMapper_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Type_Queue_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Queue_1_ILTypeInstance_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            System_Action_1_Single_Binding.Register(app);
            System_Threading_Tasks_TaskCompletionSource_1_Boolean_Binding.Register(app);
            System_Threading_CancellationToken_Binding.Register(app);
            System_Runtime_CompilerServices_AsyncVoidMethodBuilder_Binding.Register(app);
            IngameDebugConsole_DebugLogConsole_Binding.Register(app);
            Blaze_Resource_ResManager_Binding.Register(app);
            Blaze_Resource_AssetBundles_AssetProviderBundle_Binding.Register(app);
            Blaze_Utility_Co_Binding.Register(app);
            ETModel_Define_Binding.Register(app);
            Blaze_Manage_Progress_ProgressManager_Binding.Register(app);
            Blaze_Common_GameSettings_Binding.Register(app);
            UnityEngine_UI_MaskableGraphic_Binding.Register(app);
            System_Single_Binding.Register(app);
            System_Threading_Monitor_Binding.Register(app);
            Sirenix_Utilities_StringExtensions_Binding.Register(app);
            System_Action_1_Int32_Binding.Register(app);
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
