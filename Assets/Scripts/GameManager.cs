using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject ArtPiece;
    
    public int TransitionLengthInSecs;

    public Text infoLabel;

    private CentrePiece centrePiece;

    private bool isPaused = false;

    private Track[] tracks;

    private int maxPlays;

    private int currentIndex = 0;

    private void UpdateSphereSettings(float strength, float roughness, Vector3 centre)
    {
        centrePiece.shapeSettings.noiseSettings.roughness = roughness;
        centrePiece.shapeSettings.noiseSettings.strength = strength;
        centrePiece.shapeSettings.noiseSettings.centre = centre;

        centrePiece.OnShapeSettingsUpdated();
    }

    private void UpdateUi(Track track)
    {
        UIManager.UpdateLabel(infoLabel, track);
    }

    void Start()
    {
        centrePiece = ArtPiece.GetComponent<CentrePiece>();

        using (var stream = File.OpenText(Path.Combine(Application.streamingAssetsPath + "/input.json")))
        {
            var serializer = new JsonSerializer();
            tracks = (Track[]) serializer.Deserialize(stream, typeof(Track[]));
        }

        SetupInitialSphere();
    }

    void SetupInitialSphere()
    {
        if (tracks != null)
        {
            maxPlays = tracks.Max(x => x.Count);

            var firstTrack = tracks[0];

            UpdateSphereSettings(
                Helpers.ConvertMonthToStrength(firstTrack.Month),
                Helpers.ConvertTotalPlaysToRoughness(firstTrack.Count, maxPlays),
                Helpers.ConvertIdToCoord(firstTrack.Id)
            );

            UpdateUi(firstTrack);
            currentIndex++;
        }
    }

    private float GetValueDifference(float oldValue, float newValue)
    {
        var difference = oldValue >= newValue ?
            oldValue - newValue:
            newValue - oldValue;

        return difference;
    }

    private Vector3 GetValueDifference(Vector3 oldValue, Vector3 newValue)
    {
        var x = oldValue.x >= newValue.x ?
            oldValue.x - newValue.x:
            newValue.x - oldValue.x;

        var y = oldValue.y >= newValue.y ?
            oldValue.y - newValue.y:
            newValue.y - oldValue.y;

        var z = oldValue.z >= newValue.z ?
            oldValue.z - newValue.z:
            newValue.z - oldValue.z;

        return new Vector3(x, y, z);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentIndex = 0;
            SetupInitialSphere();
        }

        if (isPaused)
        {
            return;
        }

        if (tracks == null)
        {
            return;
        }

        var newTrack = tracks[currentIndex];
        var prevTrack = tracks[currentIndex - 1];

        var prevStrength = Helpers.ConvertMonthToStrength(prevTrack.Month);
        var newStrength = Helpers.ConvertMonthToStrength(newTrack.Month);
        var strengthDifference = GetValueDifference(prevStrength, newStrength);
        var strengthPerFrames = strengthDifference / TransitionLengthInSecs * Time.deltaTime;

        var prevRoughness = Helpers.ConvertTotalPlaysToRoughness(prevTrack.Count, maxPlays);
        var newRoughness = Helpers.ConvertTotalPlaysToRoughness(newTrack.Count, maxPlays);
        var roughnessDifference = GetValueDifference(prevRoughness, newRoughness);
        var roughnessPerFrames = roughnessDifference / TransitionLengthInSecs * Time.deltaTime;

        var prevCoords = Helpers.ConvertIdToCoord(prevTrack.Id);
        var newCoords = Helpers.ConvertIdToCoord(newTrack.Id);
        var coordDifference = GetValueDifference(prevCoords, newCoords);
        var coordsPerFrames = coordDifference / TransitionLengthInSecs * Time.deltaTime;

        if ((Math.Round(GetValueDifference(centrePiece.shapeSettings.noiseSettings.strength, newStrength), 1) == 0)
            && Math.Round(GetValueDifference(centrePiece.shapeSettings.noiseSettings.roughness, newRoughness), 1) == 0
            && Math.Round(GetValueDifference(centrePiece.shapeSettings.noiseSettings.centre.x, newCoords.x), 1) == 0
            && Math.Round(GetValueDifference(centrePiece.shapeSettings.noiseSettings.centre.y, newCoords.y), 1) == 0
            && Math.Round(GetValueDifference(centrePiece.shapeSettings.noiseSettings.centre.z, newCoords.z), 1) == 0)
        {
            if (currentIndex < tracks.Length)
            {
                currentIndex++;
            } else
            {
                currentIndex = 0;
            }

            UpdateUi(tracks[currentIndex]);

            return;
        }
        
        if (newStrength < centrePiece.shapeSettings.noiseSettings.strength)
        {
            centrePiece.shapeSettings.noiseSettings.strength = centrePiece.shapeSettings.noiseSettings.strength - strengthPerFrames;
        } else
        {
            centrePiece.shapeSettings.noiseSettings.strength = centrePiece.shapeSettings.noiseSettings.strength + strengthPerFrames;
        }

        if (newRoughness < centrePiece.shapeSettings.noiseSettings.roughness)
        {
            centrePiece.shapeSettings.noiseSettings.roughness = centrePiece.shapeSettings.noiseSettings.roughness - roughnessPerFrames;
        } else
        {
            centrePiece.shapeSettings.noiseSettings.roughness = centrePiece.shapeSettings.noiseSettings.roughness + roughnessPerFrames;
        }

        if (newCoords.x < centrePiece.shapeSettings.noiseSettings.centre.x)
        {
            centrePiece.shapeSettings.noiseSettings.centre.x = centrePiece.shapeSettings.noiseSettings.centre.x - coordsPerFrames.x;
        } else
        {
            centrePiece.shapeSettings.noiseSettings.centre.x = centrePiece.shapeSettings.noiseSettings.centre.x + coordsPerFrames.x;
        }

        if (newCoords.y < centrePiece.shapeSettings.noiseSettings.centre.y)
        {
            centrePiece.shapeSettings.noiseSettings.centre.y = centrePiece.shapeSettings.noiseSettings.centre.y - coordsPerFrames.y;
        } else
        {
            centrePiece.shapeSettings.noiseSettings.centre.y = centrePiece.shapeSettings.noiseSettings.centre.y + coordsPerFrames.y;
        }

        if (newCoords.z < centrePiece.shapeSettings.noiseSettings.centre.z)
        {
            centrePiece.shapeSettings.noiseSettings.centre.z = centrePiece.shapeSettings.noiseSettings.centre.z - coordsPerFrames.z;
        } else
        {
            centrePiece.shapeSettings.noiseSettings.centre.z = centrePiece.shapeSettings.noiseSettings.centre.z + coordsPerFrames.z;
        }

        centrePiece.OnShapeSettingsUpdated();
    }
}
