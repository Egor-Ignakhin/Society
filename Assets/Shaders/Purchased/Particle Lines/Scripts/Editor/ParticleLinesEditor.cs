//	Unluck Software	
// 	www.chemicalbliss.com
#pragma warning disable 0618

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ParticleLines))]
[System.Serializable]
public class ParticleLinesEditor : Editor {
	public string[] options = new string[] { "None", "Lifetime", "Distance" };
	public int index = 0;
	public string sortText;
	public bool showHelp;
	public string[] optionsLight = new string[] { "None", "Random", "End" };
	public int indexLight = 0;
	SerializedProperty startGradient;
	SerializedProperty endGradient;

	public void OnEnable() {
		var target_cs = (ParticleLines)target;
		if (target_cs._sortParticleOnLife) index = 1;
		else if (target_cs._sortParticleOnDistance) index = 2;
		else index = 0;
		CheckIndex();
		if (target_cs._positionLight == "random") indexLight = 1;
		else if (target_cs._positionLight == "end") indexLight = 2;
		else indexLight = 0;
	}

	public Color dColor = new Color32((byte)200, (byte)200, (byte)200, (byte)255);
	public Color aColor = Color.white;
	public Color bColor = new Color32((byte)222, (byte)222, (byte)222, (byte)255);
	public Color helpColor = new Color32((byte)100, (byte)100, (byte)100, (byte)255);
	public GUIStyle helpStyle;
	public GUIStyle buttonStyle;

	public override void OnInspectorGUI() {
		if (EditorGUIUtility.isProSkin == true)	
			helpColor = Color.cyan; //new Color32((byte)150, (byte)150, (byte)150, (byte)255);
		var target_cs = (ParticleLines)target;
		GUI.color = bColor;
		helpStyle = new GUIStyle(GUI.skin.label);
		helpStyle.fontSize = 9;
		helpStyle.normal.textColor = helpColor;
		buttonStyle = new GUIStyle(GUI.skin.button);
		buttonStyle.fontStyle = FontStyle.Bold;
		buttonStyle.fixedWidth = 25.0f;
		helpStyle.wordWrap = true;

		BeginBox();

		EditorGUILayout.BeginHorizontal();
		string exeButtonText = "Edit Mode (OFF)";
		if (target_cs.executeInEditMode){
			GUI.color = Color.cyan;
			exeButtonText = "Edit Mode (ON)";
		}
		if (GUILayout.Button(exeButtonText, GUILayout.Width(125))) {
			target_cs.executeInEditMode = !target_cs.executeInEditMode;
			target_cs._ps.Clear();
		}

		GUI.color = aColor;
		
		string helpButtonText = "Help (OFF)";
		if (showHelp) {
			GUI.color = Color.cyan;
			helpButtonText = "Help (ON)";
		}
		if (GUILayout.Button(helpButtonText)) showHelp = !showHelp;

		EndBox();
		BeginBox();

		ShowHelp("Tip: Use 2 inspectors to edit line and particles at the same time.");

		target_cs._line = EditorGUILayout.ObjectField("Line Renderer", target_cs._line, typeof(SimpleLineRenderer), true) as SimpleLineRenderer;
		target_cs.lineMaterial = EditorGUILayout.ObjectField("Line Material", target_cs.lineMaterial, typeof(Material), true) as Material;
		target_cs._ps = EditorGUILayout.ObjectField("Particle System", target_cs._ps, typeof(ParticleSystem), true) as ParticleSystem;
		index = EditorGUILayout.Popup("Sorting On Particle", index, options);
		if (GUI.changed) {
			CheckIndex();
		}
		if (target_cs._sortParticleOnLife || target_cs._sortParticleOnDistance) {
			EditorGUILayout.LabelField(sortText);
			EditorGUILayout.LabelField("Sort Interval");
			target_cs._sortInterval = (int)EditorGUILayout.Slider((float)target_cs._sortInterval, 0.0f, 50.0f);
			if (target_cs._sortInterval <= 1) {
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.LabelField("Sorting every frame impacts performance", EditorStyles.miniBoldLabel);
				EditorGUILayout.EndVertical();
			} else if (target_cs._sortInterval >= 3) {
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.TextArea("High sorting interval might cause stuttering\nWill tween positions between particles\n(try high number for better performance)", EditorStyles.miniBoldLabel);
				EditorGUILayout.EndVertical();
			}
			ShowHelp("Increase performance by limit frame sorting");
			ShowHelp("(Higher = Better Performance)");
		}

		target_cs._lineResolution = EditorGUILayout.IntField("_lineResolution", target_cs._lineResolution);
		if (target_cs._lineResolution < 10) target_cs._lineResolution = 10;

		target_cs._cutEndSegments = EditorGUILayout.IntField("Cut End Segments", target_cs._cutEndSegments);
		if (target_cs._cutEndSegments < 0) target_cs._cutEndSegments = 0;

		target_cs._freezeZeroParticle = EditorGUILayout.Toggle("Freeze First Segment", target_cs._freezeZeroParticle);
		ShowHelp("The first line segment will be centered to particle system");

		EndBox();
		BeginBox();

		target_cs._light = EditorGUILayout.ObjectField("Light Object", target_cs._light, typeof(Light), true) as Light;
		ShowHelp("Assign Light used in effect");
		if (target_cs._light != null) {
			target_cs._vertexCountIntensity = EditorGUILayout.Toggle("Particle Intensity", target_cs._vertexCountIntensity);
			ShowHelp("Use the amount of particles to decide how bright the lights are");

			if (target_cs._vertexCountIntensity) {
				target_cs._vertexCountIntensityMultiplier = EditorGUILayout.FloatField("Intensity Multiplier", target_cs._vertexCountIntensityMultiplier);
				ShowHelp("Multiply the intensity of the light");
			}

			target_cs._flicker = EditorGUILayout.Toggle("Flicker Light", target_cs._flicker);
			ShowHelp("Flicker intensity based on a animation curve");

			if (target_cs._flicker) {
				if (target_cs._lightFlicker.length == 0) target_cs._lightFlicker = new AnimationCurve(new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 1.0f));
				target_cs._lightFlicker = EditorGUILayout.CurveField(target_cs._lightFlicker); EditorGUILayout.BeginVertical("Box");
			}

			indexLight = EditorGUILayout.Popup("Light Position", indexLight, optionsLight);
			ShowHelp("Position lights based on particle positions");

			if (GUI.changed)
				target_cs._positionLight = optionsLight[indexLight].ToLower();

		}

		EndBox();
		BeginBox();

		target_cs.startColor = EditorGUILayout.ColorField("Start Color", target_cs.startColor);
		target_cs.endColor = EditorGUILayout.ColorField("End Color", target_cs.endColor);

		if (target_cs._gradients) {
			GUI.color = Color.cyan;
			if (GUILayout.Button("Gradients (ON)")) {
				target_cs._gradients = !target_cs._gradients;
			}
			GUI.color = aColor;
			EditorGUI.BeginChangeCheck();

			SerializedObject serializedObject = new SerializedObject(target);
			startGradient = serializedObject.FindProperty("_gradientStart");
			endGradient = serializedObject.FindProperty("_gradientEnd");
			ShowHelp("Gradient used over time to change the START color of the line");
			EditorGUILayout.PropertyField(startGradient, new GUIContent("Start Gradient"));
			EditorGUILayout.PropertyField(endGradient, new GUIContent("End Gradient"));

			if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();

			ShowHelp("Gradient used over time to change the END color of the line");

			target_cs._gradientSpeed = EditorGUILayout.FloatField("Cycle Speed", target_cs._gradientSpeed);
			ShowHelp("How fast colors are cycled");

			target_cs._randomGradientStart = EditorGUILayout.Toggle("Random Cycle Start", target_cs._randomGradientStart);
			ShowHelp("Starts the gradient at a random position");

			target_cs._gradientLight = EditorGUILayout.Toggle("Gradient Light Color", target_cs._gradientLight);
			ShowHelp("Apply gradient colors to light");
		} else {
			
			if (GUILayout.Button("Gradients (OFF)")) {
				target_cs._gradients = !target_cs._gradients;
			}
		}

		EndBox();
		BeginBox();

		if (target_cs._tileLineMaterial) {
			GUI.color = Color.cyan;
			if (GUILayout.Button("Tile Line Material (ON)"))
				target_cs._tileLineMaterial = !target_cs._tileLineMaterial;
			GUI.color = aColor;

			target_cs._tileMultiplier = EditorGUILayout.FloatField("Tile Multiplier", target_cs._tileMultiplier);
			ShowHelp("Tile material based on vertex length * multplier");

			target_cs._fixedTileMaterial = EditorGUILayout.Toggle("Fixed Tile Material", target_cs._fixedTileMaterial);
			ShowHelp("Tiling only based on multiplier");

			target_cs._tileAnimate = EditorGUILayout.Toggle("Animate Material Tile", target_cs._tileAnimate);
			ShowHelp("Animate tiling of material over time");

			if (target_cs._tileAnimate)
				target_cs._tileAnimateSpeed = EditorGUILayout.FloatField("Animate Speed", target_cs._tileAnimateSpeed);
		} else {
			
			if (GUILayout.Button("Tile Line Material (OFF)"))
				target_cs._tileLineMaterial = !target_cs._tileLineMaterial;

		}

		EndBox();
		BeginBox();

		target_cs.startScaleMultiplier = EditorGUILayout.FloatField("Start Scale Multiplier", target_cs.startScaleMultiplier);
		ShowHelp("How much to scale the start of the line");

		target_cs.endScaleMultiplier = EditorGUILayout.FloatField("End Scale Multiplier", target_cs.endScaleMultiplier);
		ShowHelp("How much to scale the end of the line");

		if (target_cs.animateScale) {
			
			GUI.color = Color.cyan;
			if (GUILayout.Button("Animate Scale (ON)"))
				target_cs.animateScale = !target_cs.animateScale;
			GUI.color = aColor;
		

			target_cs.startScale = EditorGUILayout.CurveField("Start", target_cs.startScale);
			ShowHelp("Start animation curve");

			target_cs.endScale = EditorGUILayout.CurveField("End", target_cs.endScale);
			ShowHelp("End animation curve");

			target_cs._scaleSpeed = EditorGUILayout.FloatField("Scale Speed", target_cs._scaleSpeed);
			ShowHelp("Speed of animation based on curves");

		} else {
			
			if (GUILayout.Button("Animate Scale (OFF)")) {
				target_cs.animateScale = !target_cs.animateScale;
			}
		}

		EndBox();
		BeginBox();

		target_cs._rotationSpeed = EditorGUILayout.Vector3Field("Rotation Speed", target_cs._rotationSpeed);
		EditorGUILayout.LabelField("(Run-time only)");

		ShowHelp("Rotate the particle system to create swirls and spirals");
		ShowHelp("(Particles must have start speed)");

		EndBox();
		EndBox();

		if (GUI.changed) {
			EditorUtility.SetDirty(target_cs);
		}
	}

	public void ShowHelp(string s) {
		if (showHelp) EditorGUILayout.LabelField(s, helpStyle);
	}

	public void BeginBox() {
		GUILayout.Space(5.0f);
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");
		GUI.color = aColor;
	}

	public void EndBox() {
		EditorGUILayout.EndVertical();
	}

	public void CheckIndex() {
		var target_cs = (ParticleLines)target;
		target_cs._sortParticleOnDistance = target_cs._sortParticleOnLife = false;
		if (index == 1) {
			sortText = "Sorting on lifetime requires more cpu";
			target_cs._sortParticleOnLife = true;
		} else if (index == 2) {
			sortText = "Sorting on distance requires alot of cpu";
			target_cs._sortParticleOnDistance = true;
		} else {
			sortText = "";
		}
	}
}