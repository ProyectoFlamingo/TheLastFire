// GENERATED AUTOMATICALLY FROM 'Assets/ScriptableObjects/InputActions/InputActions_Main.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Flamingo
{
    public class InputMaster : IInputActionCollection, IDisposable
    {
        private InputActionAsset asset;
        public InputMaster()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputActions_Main"",
    ""maps"": [
        {
            ""name"": ""Player_Mateo"",
            ""id"": ""cb325c07-1816-489f-8b4b-6c71af8ec295"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""2ee33b5c-888b-41ca-9ab3-6363c0607c7e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""FireShooting_Frontal"",
                    ""type"": ""Button"",
                    ""id"": ""66caf3e6-41b0-4d69-b67b-0840ac28e3b6"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack_Sword"",
                    ""type"": ""Button"",
                    ""id"": ""1144f27a-1f3d-47a8-befd-0eb3d9f57d91"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftAxes"",
                    ""type"": ""Button"",
                    ""id"": ""ac584050-0443-40c1-a687-46ad064b09dc"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightAxes"",
                    ""type"": ""Button"",
                    ""id"": ""126aa351-12c3-4945-904d-7d84815aae03"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""91efcaaa-db60-471e-b985-1cd5ff9a307a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bf3f1ae4-4613-4512-9d66-03c4a07e0922"",
                    ""path"": ""<NPad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba28b3d3-a3f9-4ad6-b1f3-a16f8dab2c48"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c218540f-8be4-4c2a-b729-853771acda7e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23a03160-66db-4b45-9d3f-62a5dd9186e9"",
                    ""path"": ""<NPad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""FireShooting_Frontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2969ed1a-08f0-4d68-94df-5fe4c622a9b4"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""FireShooting_Frontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a2724cc-4534-42fa-9178-3555e636e618"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""FireShooting_Frontal"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c95dfe25-8a28-4791-919b-a52e89ad417f"",
                    ""path"": ""<XInputController>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""Attack_Sword"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da969260-53b4-4148-a787-f4cfdac60e18"",
                    ""path"": ""<NPad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""Attack_Sword"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2de3f86-89ff-4353-b597-0473daa413a8"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Attack_Sword"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Left-Stick [Xbox Controller]"",
                    ""id"": ""595d6579-c029-485c-8a62-58b740ed661e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6799680c-2e9f-49c9-b419-d849820469cd"",
                    ""path"": ""<XInputController>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bc224ace-6e6d-4299-96e2-e717dad21213"",
                    ""path"": ""<XInputController>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""c094b166-edb6-49c2-b564-501cccb40852"",
                    ""path"": ""<XInputController>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""e7d360c9-f31a-4ead-864c-cfe2d8faaf03"",
                    ""path"": ""<XInputController>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left-Stick [Switch Controller]"",
                    ""id"": ""b8c8d6c6-2489-4f72-821c-1cf3e9a3ca14"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""08e76c3c-3e4e-42c1-9747-b223cdeea6f4"",
                    ""path"": ""<NPad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ca275f95-3a1a-4ca2-91f6-bb228ddc80d7"",
                    ""path"": ""<NPad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""91f687ef-9c9c-495f-aa6a-7504be303985"",
                    ""path"": ""<NPad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d98d0385-5453-453d-9a0c-38dd5eed798e"",
                    ""path"": ""<NPad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD [Keyboard]"",
                    ""id"": ""c43ee69e-5853-4e3b-86ec-bb526fa6a000"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6a14a88f-9cd9-4e76-8cfd-2249183f1c5d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""040098d5-d752-4737-8c21-f0ce95a616b8"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1d71a5b1-bf74-475d-986e-9257d3d5bd67"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""74507a7d-c11e-4134-a73a-05fe0d335b83"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D-Pad [Keyboard]"",
                    ""id"": ""24369d96-4c93-4acf-aa93-76fda3ee422e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c0713505-266a-4294-a24b-e89cafa60153"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b5434961-7b15-4c84-aa7f-266d1b2cbac6"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8a72e523-9a3d-4f50-b1aa-2d1b05009278"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""a2af0815-54f0-4653-8bd5-b53bcac11bcc"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right-Stick [Switch Controller]"",
                    ""id"": ""ca3e4785-163a-476f-a3cb-9a13b863ca3b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightAxes"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""3d34d093-61d6-4553-9f9c-8e4219bd1422"",
                    ""path"": ""<NPad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9d616578-dda9-4380-ad06-7fbbac19495c"",
                    ""path"": ""<NPad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3076e047-2d8a-4904-bf1e-6f226786ba24"",
                    ""path"": ""<NPad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4e306c64-1b20-48eb-b480-9a329505db6d"",
                    ""path"": ""<NPad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right-Stick [Xbox Controller]"",
                    ""id"": ""c332edc7-7280-4f04-989b-efef9d09c0cb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightAxes"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5f4c30e7-f386-472b-8ddc-95b3a3dd12dd"",
                    ""path"": ""<XInputController>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e54aca7b-81c2-4b48-b185-1205f658af00"",
                    ""path"": ""<XInputController>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4ef6e7c0-5178-4804-8548-b3947316b64b"",
                    ""path"": ""<XInputController>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9d648bdf-f8ee-4315-8b99-18fd95c8783d"",
                    ""path"": ""<XInputController>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Mouse"",
                    ""id"": ""4c49efc9-ce42-4a5b-8c6c-ba249cc704a5"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightAxes"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b861d003-ec45-4fe6-afd6-a2f9783a0792"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3b67a13d-0b3b-4116-9e7c-a35beec4f444"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e2f758e0-3d21-480a-8696-292c7e439efc"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""36354b58-a859-44ae-b7b8-c2508e43813d"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""RightAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3d1724e4-28d7-446d-a794-01593384ffee"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50862e63-55c4-432d-b5f9-ca8d34c0cb07"",
                    ""path"": ""<XInputController>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6041239a-8d57-4c35-a403-9b2a1e459bd1"",
                    ""path"": ""<NPad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Character_Destino"",
            ""id"": ""495213c2-f477-4bd0-b01d-45ecc05a0fdf"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""6de48d56-ad27-44a7-a4f0-795a1df184a8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftAxes"",
                    ""type"": ""Button"",
                    ""id"": ""3fe1fc98-e08a-40e6-9c0f-3a27aa188bde"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0328f304-0bcd-4d79-9823-3224e1dfdeef"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""D-Pad [Keyboard]"",
                    ""id"": ""7073d5c8-7bf7-4c2d-ae10-cab3485cdb7d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""116b9e55-fe57-4e23-8176-033667e2ec42"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""589a466f-f0cc-497e-be62-bc68b9bb2d4c"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a5fad9b0-dcf1-4101-b489-1140f3bc9e89"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4e243006-bced-45bc-acda-2947e36d993c"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WASD [Keyboard]"",
                    ""id"": ""86b42f04-9632-451c-926c-0a6ab77ae2d6"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""4a9feb61-f7bb-480b-a44a-6ef11f2a878e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c2d5f736-394a-48a9-800b-eac7229be0ac"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""fc70ba84-dec2-4f13-a7ae-e0b959d406be"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4fa992b5-d53b-4ff2-9ef3-78c8882ff6e4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""LeftAxes"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""46913f62-a875-482d-9e1d-ce070965d623"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""481d0f4f-883d-465b-83cc-ccdb175b72fd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""Button"",
                    ""id"": ""c8474ccd-427b-42e9-8608-e95e7d630235"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""635782a1-4dbe-4f98-aca0-a67e4a0f511f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""340ff070-6ac7-4463-b880-f10fc24bb5fe"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7b48f1aa-54e0-4129-9068-a1ec1c8209ff"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""8dfa2cf8-35b9-4a1d-9d2b-49ecea17e5d4"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1008e46c-56ff-4508-961d-c2b956e2001f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6ca1000b-4016-45c3-9040-b67f14248230"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""D-Pad"",
                    ""id"": ""734e40ad-b2eb-4123-987c-684e08e8a4ff"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cb370a2b-cc04-459f-a6e8-a25c399d84f4"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2c26135b-bf81-44fd-ba98-3323094fef7f"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""48bf7c3c-9ff1-44f9-8a57-af92320813be"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""523accac-7a83-4265-8e9b-989f11461c62"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2DVector"",
                    ""id"": ""b4f7617a-5542-4954-affd-584e908ebecb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""cc07d59e-c047-410a-bb77-c92d1497516c"",
                    ""path"": ""<XInputController>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bae1cbb4-26cf-4540-92db-d50791ac7799"",
                    ""path"": ""<XInputController>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a2c8b981-4ba7-41be-a65b-8b9d48393473"",
                    ""path"": ""<XInputController>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""575f58cd-90c5-4309-9239-5bf1e5e450ed"",
                    ""path"": ""<XInputController>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftStick"",
                    ""id"": ""9aa2c963-2452-4ecf-ad68-f6aa252e87ef"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0700a149-7126-4f5e-bc0b-94f149f7caa2"",
                    ""path"": ""<NPad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ac803590-5e6a-4ea7-b2df-0912310c28e1"",
                    ""path"": ""<NPad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d7409203-5d1d-43ef-a88e-e3fb58158e97"",
                    ""path"": ""<NPad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9c8702c1-46c4-420e-bcdf-e9589b6e7001"",
                    ""path"": ""<NPad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""811e3f09-b2b4-4e35-94f2-07deae644aae"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""275d36c0-4bbb-4df0-b0e5-f32ef7c9a5f9"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25baad6b-7e40-4c12-8d7b-9342495307fb"",
                    ""path"": ""<NPad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b89c87a-bb53-402d-83a5-620c1a179526"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControlScheme_Keyboard&Mouse"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b89df382-a507-495d-a5ca-7cdc5a450fe3"",
                    ""path"": ""<XInputController>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_Xbox360"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80e7e018-e724-49eb-a6ca-3f431e79f135"",
                    ""path"": ""<NPad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""ControllerScheme_NintendoSwitch"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""ControllerScheme_NintendoSwitch"",
            ""bindingGroup"": ""ControllerScheme_NintendoSwitch"",
            ""devices"": [
                {
                    ""devicePath"": ""<NPad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""ControllerScheme_Xbox360"",
            ""bindingGroup"": ""ControllerScheme_Xbox360"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""ControlScheme_Keyboard&Mouse"",
            ""bindingGroup"": ""ControlScheme_Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Player_Mateo
            m_Player_Mateo = asset.FindActionMap("Player_Mateo", throwIfNotFound: true);
            m_Player_Mateo_Jump = m_Player_Mateo.FindAction("Jump", throwIfNotFound: true);
            m_Player_Mateo_FireShooting_Frontal = m_Player_Mateo.FindAction("FireShooting_Frontal", throwIfNotFound: true);
            m_Player_Mateo_Attack_Sword = m_Player_Mateo.FindAction("Attack_Sword", throwIfNotFound: true);
            m_Player_Mateo_LeftAxes = m_Player_Mateo.FindAction("LeftAxes", throwIfNotFound: true);
            m_Player_Mateo_RightAxes = m_Player_Mateo.FindAction("RightAxes", throwIfNotFound: true);
            m_Player_Mateo_Pause = m_Player_Mateo.FindAction("Pause", throwIfNotFound: true);
            // Character_Destino
            m_Character_Destino = asset.FindActionMap("Character_Destino", throwIfNotFound: true);
            m_Character_Destino_Jump = m_Character_Destino.FindAction("Jump", throwIfNotFound: true);
            m_Character_Destino_LeftAxes = m_Character_Destino.FindAction("LeftAxes", throwIfNotFound: true);
            // UI
            m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
            m_UI_Move = m_UI.FindAction("Move", throwIfNotFound: true);
            m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
            m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
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

        // Player_Mateo
        private readonly InputActionMap m_Player_Mateo;
        private IPlayer_MateoActions m_Player_MateoActionsCallbackInterface;
        private readonly InputAction m_Player_Mateo_Jump;
        private readonly InputAction m_Player_Mateo_FireShooting_Frontal;
        private readonly InputAction m_Player_Mateo_Attack_Sword;
        private readonly InputAction m_Player_Mateo_LeftAxes;
        private readonly InputAction m_Player_Mateo_RightAxes;
        private readonly InputAction m_Player_Mateo_Pause;
        public struct Player_MateoActions
        {
            private InputMaster m_Wrapper;
            public Player_MateoActions(InputMaster wrapper) { m_Wrapper = wrapper; }
            public InputAction @Jump => m_Wrapper.m_Player_Mateo_Jump;
            public InputAction @FireShooting_Frontal => m_Wrapper.m_Player_Mateo_FireShooting_Frontal;
            public InputAction @Attack_Sword => m_Wrapper.m_Player_Mateo_Attack_Sword;
            public InputAction @LeftAxes => m_Wrapper.m_Player_Mateo_LeftAxes;
            public InputAction @RightAxes => m_Wrapper.m_Player_Mateo_RightAxes;
            public InputAction @Pause => m_Wrapper.m_Player_Mateo_Pause;
            public InputActionMap Get() { return m_Wrapper.m_Player_Mateo; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(Player_MateoActions set) { return set.Get(); }
            public void SetCallbacks(IPlayer_MateoActions instance)
            {
                if (m_Wrapper.m_Player_MateoActionsCallbackInterface != null)
                {
                    Jump.started -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnJump;
                    Jump.performed -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnJump;
                    Jump.canceled -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnJump;
                    FireShooting_Frontal.started -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnFireShooting_Frontal;
                    FireShooting_Frontal.performed -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnFireShooting_Frontal;
                    FireShooting_Frontal.canceled -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnFireShooting_Frontal;
                    Attack_Sword.started -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnAttack_Sword;
                    Attack_Sword.performed -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnAttack_Sword;
                    Attack_Sword.canceled -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnAttack_Sword;
                    LeftAxes.started -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnLeftAxes;
                    LeftAxes.performed -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnLeftAxes;
                    LeftAxes.canceled -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnLeftAxes;
                    RightAxes.started -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnRightAxes;
                    RightAxes.performed -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnRightAxes;
                    RightAxes.canceled -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnRightAxes;
                    Pause.started -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnPause;
                    Pause.performed -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnPause;
                    Pause.canceled -= m_Wrapper.m_Player_MateoActionsCallbackInterface.OnPause;
                }
                m_Wrapper.m_Player_MateoActionsCallbackInterface = instance;
                if (instance != null)
                {
                    Jump.started += instance.OnJump;
                    Jump.performed += instance.OnJump;
                    Jump.canceled += instance.OnJump;
                    FireShooting_Frontal.started += instance.OnFireShooting_Frontal;
                    FireShooting_Frontal.performed += instance.OnFireShooting_Frontal;
                    FireShooting_Frontal.canceled += instance.OnFireShooting_Frontal;
                    Attack_Sword.started += instance.OnAttack_Sword;
                    Attack_Sword.performed += instance.OnAttack_Sword;
                    Attack_Sword.canceled += instance.OnAttack_Sword;
                    LeftAxes.started += instance.OnLeftAxes;
                    LeftAxes.performed += instance.OnLeftAxes;
                    LeftAxes.canceled += instance.OnLeftAxes;
                    RightAxes.started += instance.OnRightAxes;
                    RightAxes.performed += instance.OnRightAxes;
                    RightAxes.canceled += instance.OnRightAxes;
                    Pause.started += instance.OnPause;
                    Pause.performed += instance.OnPause;
                    Pause.canceled += instance.OnPause;
                }
            }
        }
        public Player_MateoActions @Player_Mateo => new Player_MateoActions(this);

        // Character_Destino
        private readonly InputActionMap m_Character_Destino;
        private ICharacter_DestinoActions m_Character_DestinoActionsCallbackInterface;
        private readonly InputAction m_Character_Destino_Jump;
        private readonly InputAction m_Character_Destino_LeftAxes;
        public struct Character_DestinoActions
        {
            private InputMaster m_Wrapper;
            public Character_DestinoActions(InputMaster wrapper) { m_Wrapper = wrapper; }
            public InputAction @Jump => m_Wrapper.m_Character_Destino_Jump;
            public InputAction @LeftAxes => m_Wrapper.m_Character_Destino_LeftAxes;
            public InputActionMap Get() { return m_Wrapper.m_Character_Destino; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(Character_DestinoActions set) { return set.Get(); }
            public void SetCallbacks(ICharacter_DestinoActions instance)
            {
                if (m_Wrapper.m_Character_DestinoActionsCallbackInterface != null)
                {
                    Jump.started -= m_Wrapper.m_Character_DestinoActionsCallbackInterface.OnJump;
                    Jump.performed -= m_Wrapper.m_Character_DestinoActionsCallbackInterface.OnJump;
                    Jump.canceled -= m_Wrapper.m_Character_DestinoActionsCallbackInterface.OnJump;
                    LeftAxes.started -= m_Wrapper.m_Character_DestinoActionsCallbackInterface.OnLeftAxes;
                    LeftAxes.performed -= m_Wrapper.m_Character_DestinoActionsCallbackInterface.OnLeftAxes;
                    LeftAxes.canceled -= m_Wrapper.m_Character_DestinoActionsCallbackInterface.OnLeftAxes;
                }
                m_Wrapper.m_Character_DestinoActionsCallbackInterface = instance;
                if (instance != null)
                {
                    Jump.started += instance.OnJump;
                    Jump.performed += instance.OnJump;
                    Jump.canceled += instance.OnJump;
                    LeftAxes.started += instance.OnLeftAxes;
                    LeftAxes.performed += instance.OnLeftAxes;
                    LeftAxes.canceled += instance.OnLeftAxes;
                }
            }
        }
        public Character_DestinoActions @Character_Destino => new Character_DestinoActions(this);

        // UI
        private readonly InputActionMap m_UI;
        private IUIActions m_UIActionsCallbackInterface;
        private readonly InputAction m_UI_Move;
        private readonly InputAction m_UI_Submit;
        private readonly InputAction m_UI_Cancel;
        public struct UIActions
        {
            private InputMaster m_Wrapper;
            public UIActions(InputMaster wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_UI_Move;
            public InputAction @Submit => m_Wrapper.m_UI_Submit;
            public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
            public InputActionMap Get() { return m_Wrapper.m_UI; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
            public void SetCallbacks(IUIActions instance)
            {
                if (m_Wrapper.m_UIActionsCallbackInterface != null)
                {
                    Move.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                    Move.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                    Move.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMove;
                    Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                    Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                    Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                    Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                    Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                    Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                }
                m_Wrapper.m_UIActionsCallbackInterface = instance;
                if (instance != null)
                {
                    Move.started += instance.OnMove;
                    Move.performed += instance.OnMove;
                    Move.canceled += instance.OnMove;
                    Submit.started += instance.OnSubmit;
                    Submit.performed += instance.OnSubmit;
                    Submit.canceled += instance.OnSubmit;
                    Cancel.started += instance.OnCancel;
                    Cancel.performed += instance.OnCancel;
                    Cancel.canceled += instance.OnCancel;
                }
            }
        }
        public UIActions @UI => new UIActions(this);
        private int m_ControllerScheme_NintendoSwitchSchemeIndex = -1;
        public InputControlScheme ControllerScheme_NintendoSwitchScheme
        {
            get
            {
                if (m_ControllerScheme_NintendoSwitchSchemeIndex == -1) m_ControllerScheme_NintendoSwitchSchemeIndex = asset.FindControlSchemeIndex("ControllerScheme_NintendoSwitch");
                return asset.controlSchemes[m_ControllerScheme_NintendoSwitchSchemeIndex];
            }
        }
        private int m_ControllerScheme_Xbox360SchemeIndex = -1;
        public InputControlScheme ControllerScheme_Xbox360Scheme
        {
            get
            {
                if (m_ControllerScheme_Xbox360SchemeIndex == -1) m_ControllerScheme_Xbox360SchemeIndex = asset.FindControlSchemeIndex("ControllerScheme_Xbox360");
                return asset.controlSchemes[m_ControllerScheme_Xbox360SchemeIndex];
            }
        }
        private int m_ControlScheme_KeyboardMouseSchemeIndex = -1;
        public InputControlScheme ControlScheme_KeyboardMouseScheme
        {
            get
            {
                if (m_ControlScheme_KeyboardMouseSchemeIndex == -1) m_ControlScheme_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("ControlScheme_Keyboard&Mouse");
                return asset.controlSchemes[m_ControlScheme_KeyboardMouseSchemeIndex];
            }
        }
        public interface IPlayer_MateoActions
        {
            void OnJump(InputAction.CallbackContext context);
            void OnFireShooting_Frontal(InputAction.CallbackContext context);
            void OnAttack_Sword(InputAction.CallbackContext context);
            void OnLeftAxes(InputAction.CallbackContext context);
            void OnRightAxes(InputAction.CallbackContext context);
            void OnPause(InputAction.CallbackContext context);
        }
        public interface ICharacter_DestinoActions
        {
            void OnJump(InputAction.CallbackContext context);
            void OnLeftAxes(InputAction.CallbackContext context);
        }
        public interface IUIActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnSubmit(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
        }
    }
}
