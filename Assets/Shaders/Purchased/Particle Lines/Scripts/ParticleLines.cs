//	By Unluck Software	
// 	www.chemicalbliss.com																																			

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[ExecuteInEditMode]
public class ParticleLines : MonoBehaviour {
#if UNITY_EDITOR
	public bool executeInEditMode = true;
#endif

	public SimpleLineRenderer _line;                        //Assign this line renderer
	public ParticleSystem _ps;                              //Assign Particle System used for line renderer

	//Sorting
	public bool _sortParticleOnLife;                        //Sorts line based on lifetime of particles
	public bool _sortParticleOnDistance = true;             //Sorts line based on distance from center
	public bool _freezeZeroParticle = true;                 //The first particle will freeze zero position, use to avoid jittering in the front of a moving trail

	public Color startColor = Color.yellow;
	public Color endColor = Color.white;

	public bool _gradients;                                 //Enable gradient colors		
	public Gradient _gradientStart;                         //Gradient color used over time to change the START color of the line
	public Gradient _gradientEnd;                           //Gradient color used over time to change the END color of the line
	public float _gradientSpeed = 1.0f;                     //How fast colors are cycled
	public bool _randomGradientStart;                       //Starts the gradient at a random position
	public bool _gradientLight;                             //Apply colors to light

	public Light _light;                                    //Assign Light used in effect
	public bool _vertexCountIntensity;                      //Use the amount of particles to decide how bright the lights are
	public float _vertexCountIntensityMultiplier = 0.01f;   //Multiply the intensity
	public bool _flicker;                                   //Flicker intensity based on a animation curve
	public AnimationCurve _lightFlicker;                    //Flicker animation curve
	public string _positionLight;                           //Position lights based on particle positions
															//"random" = finds a random particle 
															//"end"	= finds a particle in the end of the line

	public bool _tileLineMaterial;                          //Enable to tile material attached to line renderer
	public float _tileMultiplier = 1.0f;                    //Tile material based on vertex length * multplier
	public bool _fixedTileMaterial;                         //Tiling only based on multiplier
	public bool _tileAnimate;                               //Animate tile material
	public float _tileAnimateSpeed;                         //Speed of animation

	public bool animateScale;                               //Enable to scale line start and end based on curves
	public float startScaleMultiplier = 1.0f;               //How much to scale the start of the line
	public AnimationCurve startScale = AnimationCurve.Linear(0, 1, 1, 1);       //Start animation curve
	public float endScaleMultiplier = 1.0f;                 //How much to scale the end of the line
	public AnimationCurve endScale = AnimationCurve.Linear(0, 1, 1, 1);                 //End animation curve
	public float _scaleSpeed = 1.0f;                        //Speed of animation based on curves
	public Vector3 _rotationSpeed;                          //Rotate the particle system to create swirls and spirals (Particles must have start speed)
	public int _sortInterval = 2;                           //Each frame sorting occurs (1=always, 2=every other frame ...) 
	int _lineVertex;                                        //Saves how many verts the line uses																																																																																																																																																																																																																																																																																																																																																																															
	ParticleSystem.Particle[] myParticles;                  //Populated with information about each particle
	ParticleSystem.Particle[] myParticlesX;
	ParticleSystem.Particle[] myParticlesY;

	float _gradientCounter;                     //Time counter for cycling colors
	float _randomNumber;                        //Random value generated at start (Used to avoid uniformed scaling and light flicker on lines that are instantiated on the same frame)
	float _saveLightIntensity;                  //Saves the initial light intensity at start

	int _randomInt;

	public int _cutEndSegments;
	public int _lineResolution = 150;                           //Reduces lenght of line to avoid looping back to start segment
	public int _currentResolution;
	public int sorted;

	public Material lineMaterial;
	public MeshRenderer cacheMeshRenderer;

#if UNITY_EDITOR
	// Particle Lines 2.0 upgrade.
	public bool upgraded;

	public void OnDrawGizmos() {
		if (upgraded) return;
		if (transform.localScale != Vector3.one) transform.localScale = Vector3.one;
		if (transform.parent.localScale != Vector3.one) transform.parent.localScale = Vector3.one;
		gameObject.name = "Line";
		if (Application.isPlaying) return;
		LineRenderer lr = GetComponent<LineRenderer>();
		if (_line == null) _line = transform.GetComponent<SimpleLineRenderer>();
		if (_line == null) _line = gameObject.AddComponent<SimpleLineRenderer>();
		cacheMeshRenderer = GetComponent<MeshRenderer>();
		if (cacheMeshRenderer == null) cacheMeshRenderer = gameObject.AddComponent<MeshRenderer>();
		MeshFilter mf = GetComponent<MeshFilter>();
		if (mf == null) mf = gameObject.AddComponent<MeshFilter>();
		if (startScaleMultiplier == -1111.0f) startScaleMultiplier = GetComponent<LineRenderer>().startWidth;
		if (endScaleMultiplier == -1111.0f) endScaleMultiplier = GetComponent<LineRenderer>().endWidth;
		if (lr) {
			lineMaterial = lr.sharedMaterial;
			startColor = lr.startColor;
			endColor = lr.endColor;
			DestroyImmediate(lr);
		}
		upgraded = true;
		EditorUtility.SetDirty(this);
	}
#endif

	public void Start() {
#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		cacheMeshRenderer = GetComponent<MeshRenderer>();
		if (cacheMeshRenderer.sharedMaterial == null) cacheMeshRenderer.sharedMaterial = Instantiate(lineMaterial);
		if (_line == null) _line = transform.GetComponent<SimpleLineRenderer>();
		if (_ps == null) _ps = transform.GetComponent<ParticleSystem>();
		if (_ps == null) _ps = transform.parent.GetComponent<ParticleSystem>();
		if (_light != null)
			_saveLightIntensity = _light.intensity;
		_randomNumber = UnityEngine.Random.value;
		_randomInt = (int)(_randomNumber * 10);
		_lightFlicker.postWrapMode = WrapMode.Loop;
		startScale.postWrapMode = WrapMode.Loop;
		endScale.postWrapMode = WrapMode.Loop;
		if (_randomGradientStart)
			_gradientCounter = _randomNumber;
	}

	public int GetFrameCount() {
		return Time.frameCount + _randomInt;    //Randomize framecount to avoid all instances of ParticleLines to sort on the same frame. (reduce performance spikes)
	}

	public int Compare(float first, float second) {
		return second.CompareTo(first);
	}

	public void SetLine() {
		if (myParticlesX == null) return;
		for (int j = 0; j < _lineVertex - _cutEndSegments; j++) {
			if (j < myParticlesX.Length)
				_line.SetPosition(j, myParticlesX[j].position);
			else
				_line.SetPosition(j, transform.position);
		}
	}

	public void SortLifetime() {
		CreateLine();
		if (myParticlesX != null) System.Array.Sort(myParticlesX, (g1, g2) => Compare(g1.remainingLifetime, g2.remainingLifetime));
	}

	public void SortDistance() {
		CreateLine();
		if (myParticlesX == null) return;
		System.Array.Sort(myParticlesX, (g1, g2) => Compare(Vector3.Distance(transform.position, g2.position), Vector3.Distance(transform.position, g1.position)));
	}

	public void CreateLine() {
		if (myParticles.Length == 0) return;
		myParticlesX = new ParticleSystem.Particle[_lineVertex];
		float n = myParticles.Length / _lineVertex;
		for (int i = 0; i < _lineVertex; i++) {
			int nn = (int)(i * n);
			if (nn < myParticles.Length) myParticlesX[i] = myParticles[nn];
		}
	}

	public void LerpLine() {
		if (myParticles.Length == 0) return;
		float n = myParticlesX.Length / _lineVertex;
		for (int i = 0; i < myParticlesX.Length; i++) {
			int nn = (int)(i * n);
			myParticlesX[i].position = Vector3.Lerp(myParticlesX[i].position, myParticlesX[Mathf.Min(nn, myParticlesX.Length - 1)].position, 1.0f);
		}
	}

	public void SetLineResolution() {
		if (_ps.particleCount < _lineResolution) _lineVertex = myParticles.Length;
		if (_lineVertex > _lineResolution) _lineVertex = _lineResolution;
		if (_lineVertex < 1) _lineVertex = 1;
		if (_lineVertex != _currentResolution) {
			_line.SetVertexCount(Mathf.Max(_lineVertex - _cutEndSegments, 0));
			if (sorted <= 10) {
				SortLifetime();
				sorted++;
			}
		}
		_currentResolution = _lineVertex;
	}

	public void LinePos() {
		myParticles = new ParticleSystem.Particle[_ps.particleCount];
		_ps.GetParticles(myParticles);
		SetLineResolution();
		if (sorted > 10 && _sortInterval > 1 && _sortParticleOnLife && _lineVertex > 2) {
			if (GetFrameCount() % _sortInterval == 0) SortLifetime();
		} else if (_sortInterval > 1 && _sortParticleOnDistance && _lineVertex > 2) {
			if (GetFrameCount() % _sortInterval == 0) SortDistance();
		} else if (_sortParticleOnLife) {
			SortLifetime();
		} else if (_sortParticleOnDistance) {
			SortDistance();
		} else {
			CreateLine();
		}
		if (_freezeZeroParticle && _lineVertex > 0)
			if ((myParticlesX != null) && myParticlesX.Length > 0) myParticlesX[0].position = _ps.transform.position;
		SetLine();
	}

	public void Update() {

#if UNITY_EDITOR
		if (!executeInEditMode && !Application.isPlaying) {
			GetComponent<MeshFilter>().mesh = null;
			return;
		}
		if (!_line || !_ps) return;
		if (cacheMeshRenderer.sharedMaterial == null) cacheMeshRenderer.sharedMaterial = Instantiate(lineMaterial);
#endif

		if (!_ps.IsAlive()) return;

		LinePos();
		if (_gradients) {
			if (_gradientCounter < 1)
				_gradientCounter += Time.deltaTime * _gradientSpeed;
			else
				_gradientCounter = 0.0f;
			startColor = _gradientStart.Evaluate(_gradientCounter);
			endColor = _gradientEnd.Evaluate(_gradientCounter);
			if ((_light != null) && _gradientLight) {
				_light.color = _gradientStart.Evaluate(_gradientCounter);
			}
		}

		_line.SetColor(startColor, endColor);

		if (_tileLineMaterial && _fixedTileMaterial) {
			cacheMeshRenderer.sharedMaterial.mainTextureScale = new Vector2(_tileMultiplier, cacheMeshRenderer.sharedMaterial.mainTextureScale.y);
		} else if (_tileLineMaterial) {
			cacheMeshRenderer.sharedMaterial.mainTextureScale = new Vector2(_lineVertex * _tileMultiplier, cacheMeshRenderer.sharedMaterial.mainTextureScale.y);
		}
		if (_tileAnimate) {
			cacheMeshRenderer.sharedMaterial.mainTextureOffset = new Vector2((cacheMeshRenderer.sharedMaterial.mainTextureOffset.x + _tileAnimateSpeed * Time.deltaTime) % 1, cacheMeshRenderer.sharedMaterial.mainTextureOffset.y);
		}
		
		if (!animateScale) {
			_line.SetWidth(startScaleMultiplier, endScaleMultiplier);
		} else {
			float t = (_ps.time * _scaleSpeed) + _randomNumber;
			_line.SetWidth(startScale.Evaluate(t) * startScaleMultiplier, endScale.Evaluate(t) * endScaleMultiplier);
		}
		if (_vertexCountIntensity)
			_light.intensity = _saveLightIntensity * _vertexCountIntensityMultiplier * _lineVertex;

#if UNITY_EDITOR
		if (!Application.isPlaying) return;
#endif
		if (_light != null) {
			if (_positionLight == "end" && _ps.particleCount > 5) {
				_light.transform.position = Vector3.Lerp(_light.transform.position, myParticles[_ps.particleCount - 6].position, Time.deltaTime * 10);
			} else if (_positionLight == "random" && _ps.particleCount > 5) {
				_light.transform.position = Vector3.Lerp(_light.transform.position, myParticles[UnityEngine.Random.Range(0, _ps.particleCount)].position, Time.deltaTime * 2);
			}

			if (_flicker) {
				_light.intensity = _lightFlicker.Evaluate(Time.time + _randomNumber) * _saveLightIntensity;
			}

		}
		if (_rotationSpeed.magnitude > 0)
			_ps.transform.Rotate(_rotationSpeed * Time.deltaTime);
	}
}