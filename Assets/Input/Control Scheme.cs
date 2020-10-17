// GENERATED AUTOMATICALLY FROM 'Assets/Input/Control Scheme.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ControlScheme : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControlScheme()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Control Scheme"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""94655498-98c2-486f-bcbd-9cb3fe61e4ba"",
            ""actions"": [
                {
                    ""name"": ""Walk"",
                    ""type"": ""Value"",
                    ""id"": ""167486d6-6b8a-4c85-9330-2b03630c7646"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Climb"",
                    ""type"": ""Value"",
                    ""id"": ""74dc9f4d-aec5-4410-9fde-96c3216bd48b"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""a4d2bc09-bbcf-444e-8b8f-4d95a0dc0d81"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""77985886-05a3-42e6-a78f-5d64639fedc2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DropIngredient"",
                    ""type"": ""Button"",
                    ""id"": ""652a0c2d-223e-4b67-9ba4-f6295630a701"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mute"",
                    ""type"": ""Button"",
                    ""id"": ""0fdbe210-f033-461e-a467-9d3c57576cf6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keys (WASD)"",
                    ""id"": ""ad2a7451-220c-4de2-a94b-8bf200d27c32"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""587bfa3f-12aa-4f00-a87b-5a46f61aaf60"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""f34c0b67-6855-4645-9ab9-d50c7f99d889"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keys (Arrows)"",
                    ""id"": ""e213e84d-7efe-4069-b074-d727d26466ac"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""e900251a-0c10-4758-9cc1-44c4e8b95303"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""d583a134-4c18-46cb-a3c9-577b9ac50183"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1e9c0701-9c89-411e-9d67-0bec9863d361"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Walk"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keys (WASD)"",
                    ""id"": ""61e30c2c-6cc8-4fad-a9a4-73bb3e962263"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ea6d83dd-8076-42c9-b1e0-706beee71458"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""60382d97-4cec-4641-9c2a-563a66595329"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keys (Arrows)"",
                    ""id"": ""a39f9a15-74cc-42a5-a9bd-b700130b06e5"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""081ff978-6e06-492b-a7e6-885535f810c8"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""00b670f7-a618-4f0b-a530-65da374111c7"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""710af15e-1da6-4b5e-a738-575941461257"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Climb"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""daebc388-9fdd-4ed5-aeb5-e31c9f3b6e26"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e40442c-6eb8-4a28-803c-009bf6e76898"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43f10a91-40f1-445f-b637-1dbfae2b64be"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26c9c733-b71d-45e5-a3f4-0f7d48cd2836"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b8202889-0932-4763-b596-cad860fc58f1"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""39312363-86e7-40dc-83ce-14d5abc3f5a7"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropIngredient"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9b92f33-8f7b-4704-8d76-ebed03486100"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropIngredient"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8b1a0d5f-c682-4833-b116-958864589265"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropIngredient"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95242e30-6af2-4dac-b9a0-8bd62825e61d"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mute"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33dcea24-717b-401d-be9e-0541a33565ce"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mute"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Walk = m_Gameplay.FindAction("Walk", throwIfNotFound: true);
        m_Gameplay_Climb = m_Gameplay.FindAction("Climb", throwIfNotFound: true);
        m_Gameplay_Attack = m_Gameplay.FindAction("Attack", throwIfNotFound: true);
        m_Gameplay_Pause = m_Gameplay.FindAction("Pause", throwIfNotFound: true);
        m_Gameplay_DropIngredient = m_Gameplay.FindAction("DropIngredient", throwIfNotFound: true);
        m_Gameplay_Mute = m_Gameplay.FindAction("Mute", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Walk;
    private readonly InputAction m_Gameplay_Climb;
    private readonly InputAction m_Gameplay_Attack;
    private readonly InputAction m_Gameplay_Pause;
    private readonly InputAction m_Gameplay_DropIngredient;
    private readonly InputAction m_Gameplay_Mute;
    public struct GameplayActions
    {
        private @ControlScheme m_Wrapper;
        public GameplayActions(@ControlScheme wrapper) { m_Wrapper = wrapper; }
        public InputAction @Walk => m_Wrapper.m_Gameplay_Walk;
        public InputAction @Climb => m_Wrapper.m_Gameplay_Climb;
        public InputAction @Attack => m_Wrapper.m_Gameplay_Attack;
        public InputAction @Pause => m_Wrapper.m_Gameplay_Pause;
        public InputAction @DropIngredient => m_Wrapper.m_Gameplay_DropIngredient;
        public InputAction @Mute => m_Wrapper.m_Gameplay_Mute;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Walk.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWalk;
                @Walk.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWalk;
                @Walk.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWalk;
                @Climb.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnClimb;
                @Climb.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnClimb;
                @Climb.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnClimb;
                @Attack.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnAttack;
                @Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                @DropIngredient.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDropIngredient;
                @DropIngredient.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDropIngredient;
                @DropIngredient.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDropIngredient;
                @Mute.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMute;
                @Mute.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMute;
                @Mute.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMute;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Walk.started += instance.OnWalk;
                @Walk.performed += instance.OnWalk;
                @Walk.canceled += instance.OnWalk;
                @Climb.started += instance.OnClimb;
                @Climb.performed += instance.OnClimb;
                @Climb.canceled += instance.OnClimb;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
                @DropIngredient.started += instance.OnDropIngredient;
                @DropIngredient.performed += instance.OnDropIngredient;
                @DropIngredient.canceled += instance.OnDropIngredient;
                @Mute.started += instance.OnMute;
                @Mute.performed += instance.OnMute;
                @Mute.canceled += instance.OnMute;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnWalk(InputAction.CallbackContext context);
        void OnClimb(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnDropIngredient(InputAction.CallbackContext context);
        void OnMute(InputAction.CallbackContext context);
    }
}
