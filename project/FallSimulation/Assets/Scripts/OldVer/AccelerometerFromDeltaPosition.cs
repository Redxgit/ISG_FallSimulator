using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AccelerometerFromDeltaPosition : MonoBehaviour {
    //[SerializeField] private PositionList listSource;

    //[SerializeField] private AccelerometerList listTarget;

    [SerializeField] private List<AccelerometerStuffStruct> listTarget;

    [SerializeField] private float offset;

    



    //& vamos a ver la idea ahora es que mas o menos parezca que no estamos haciendo cosas raras 
    // y que parezca que estamos trabajando, o bueno podemos hacer la cronica del dia tal y como hice cuando vino

        /// <summary>
        /// me paso a un summary que es más comodo cuando vino la competencia de TVE a matadero cuando estabamos 
        /// haciendo el juego y demas pero a ver que plan de vida
        /// por lo menos malola ha hecho bastante bien la entrevista
        /// </summary>
    private void Start() { listTarget = new List<AccelerometerStuffStruct>(); }

    public void ResetThings() { listTarget = new List<AccelerometerStuffStruct>(); }

    public List<AccelerometerStuffStruct> GetListTarget { get { return listTarget; } }


    public void WriteInfoToFile(string newPath = "") {


        string date_time = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        
        string fileName = date_time + "_" + GameManager.Instance.FileName;


        if (newPath != "") {
            fileName = newPath;            
        }
        
        bool exists = Directory.Exists(Application.streamingAssetsPath + "/outcsvs/");

        if(!exists)
            Directory.CreateDirectory(Application.streamingAssetsPath + "/outcsvs/");

        string fullpath = Application.streamingAssetsPath + "/outcsvs/" + fileName + ".csv";

        Debug.Log("PrintingStuff to " + fullpath);


        List<string> fullString = new List<string>();

        if (File.Exists(fullpath)) {
            File.Delete(fullpath);
        }

        fullString.Add("count,delta,accelx,accely,accelz,pos1x,pos1y,pos1z,pos2x,pos2y,pos2z,gforcex,gforcey,gforcez,movementType");

        for (int i = 0; i < listTarget.Count; i++) {
            fullString.Add(listTarget[i].timestamp.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].deltaTime.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].values.x.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].values.y.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].values.z.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].pos1.x.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].pos1.y.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].pos1.z.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].pos2.x.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].pos2.y.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].pos2.z.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].gforce.x.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].gforce.y.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].gforce.z.ToString("F6").Replace(',', '.') + ',' +
                listTarget[i].movementType);
        }

        File.WriteAllLines(fullpath, fullString);

        Debug.Log("Done");
    }

    public void CreateAccelerometerFromDeltaPos(List<DeltaPositionsStruct> listSource) {
        for (int i = 0; i < listSource.Count - 1; i++) {
            float dt = listSource[i + 1].deltaTime - listSource[i].deltaTime;
            Vector3 dpos = listSource[i + 1].values - listSource[i].values;
            Vector3 sumgforce = (listSource[i + 1].gForce + listSource[i].gForce).normalized;
            Vector3 accelerometerValue = (dpos / (dt )) + (sumgforce * offset);
            AccelerometerStuffStruct newStuff = new AccelerometerStuffStruct(Time.time, dt, accelerometerValue, listSource[i + 1].values, listSource[i].values, sumgforce, listSource[i].movementType);

            listTarget.Add(newStuff);
        }
    }

    public void CreateAccelerometerListFromDeltaPos(List<DeltaPositionsStruct> listSource, int pos) {

        float dt = listSource[pos + 1].timestamp - listSource[pos].timestamp;
        Vector3 dpos = listSource[pos + 1].values - listSource[pos].values;
        Vector3 sumgforce = (listSource[pos + 1].gForce + listSource[pos].gForce).normalized;
        Vector3 accelerometerValue = (dpos / (dt )) + (sumgforce * offset);
        AccelerometerStuffStruct newStuff = new AccelerometerStuffStruct(Time.time, dt, accelerometerValue, listSource[pos + 1].values, listSource[pos].values, sumgforce, listSource[pos].movementType);
        //AccelerometerStuffStruct newStuff = new AccelerometerStuffStruct(pos, dt, accelerometerValue, dpos);
        //Debug.Log("dt: " + dt + " accelerometerValue: " + accelerometerValue+ " dpos: " + dpos);
        listTarget.Add(newStuff);

        //File.AppendAllText(fullpath, "count,delta,x,y,z");
        /*
        for (int i = 0; i < listTarget.positionList.Count; i++) {
            string fullstr = listTarget.positionList[i].count.ToString() + ',' +
                             listTarget.positionList[i].deltaTime.ToString("F6") + ',' +
                             listTarget.positionList[i].values.ToString("F6"); 
            File.AppendAllText(fullpath, fullstr);    
        }*/
    }
}