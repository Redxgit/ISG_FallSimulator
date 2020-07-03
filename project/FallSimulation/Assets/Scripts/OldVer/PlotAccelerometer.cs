using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotAccelerometer : MonoBehaviour {

    [SerializeField] private LineRenderer accelX;
    [SerializeField] private LineRenderer accelY;
    [SerializeField] private LineRenderer accelZ;

    [SerializeField] private AccelerometerFromDeltaPosition infoToPlot;

    [SerializeField] private int maxPointsToPlot;

    [SerializeField] private Transform eq10, eqm10, maxwidth;

    private Vector3[] accelXPos, accelYPos, accelZPos;

    private float feq10, feqm10, fmaxwidth;
    [SerializeField] private float remapmin = -100, remapmax = 100;
    // Start is called before the first frame update

    private void Start() {
        accelXPos = new Vector3[maxPointsToPlot];
        accelYPos = new Vector3[maxPointsToPlot];
        accelZPos = new Vector3[maxPointsToPlot];
        fmaxwidth = maxwidth.localPosition.x;

        feq10 = eq10.localPosition.y;
        feqm10 = eqm10.localPosition.y;        
    }

    private void LateUpdate() {
        UpdatePlot();
    }

    [ContextMenu("WTH")]
    private void TestRemap() {
        float valToRemap = 9.81f;
        Debug.Log("Way 1 " + ExtensionMethods.Remap(valToRemap, -100, 100, feqm10, feq10));
        Debug.Log("Way 2 " + ExtensionMethods.Remap(valToRemap, 100, feq10, -100, feqm10));
    }

    public void UpdatePlot() {
        int maxLen = infoToPlot.GetListTarget.Count;

        if (maxLen < maxPointsToPlot) {
            accelX.positionCount = maxLen;
            accelY.positionCount = maxLen;
            accelZ.positionCount = maxLen;
        }

        if (maxLen == 0) return;

        int from = 0;

        if (maxLen > maxPointsToPlot) {
            from = maxLen - maxPointsToPlot;
        }

        for (int i = from, j = 0; i < maxLen; ++j, i++) {
            Vector3 currVal = infoToPlot.GetListTarget[i].values;

            accelXPos[j] = new Vector3(
                j * (fmaxwidth / (float)maxPointsToPlot),
                ExtensionMethods.Remap(currVal.x, remapmin, remapmax, feqm10, feq10),
                0);

            accelYPos[j] = new Vector3(
                j * (fmaxwidth / (float)maxPointsToPlot),
                ExtensionMethods.Remap(currVal.y, remapmin, remapmax, feqm10, feq10),
                0);

            accelZPos[j] = new Vector3(
                j * (fmaxwidth / (float)maxPointsToPlot),
                ExtensionMethods.Remap(currVal.z, remapmin, remapmax, feqm10, feq10),
                0);
        }
        accelX.SetPositions(accelXPos);
        accelY.SetPositions(accelYPos);
        accelZ.SetPositions(accelZPos);
    }

}
