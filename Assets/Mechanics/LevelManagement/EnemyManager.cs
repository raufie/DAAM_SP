using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject [] Enemies;
    private string enemyCode;
    private LevelData levelData;



    public void LoadEnemies(){
        
        Enemies = GetComponent<LevelManager>().Enemies;
        enemyCode = GetComponent<LevelManager>().levelParser.data.enemies;
        levelData = GetComponent<LevelManager>().levelParser.data;

        /*
        a: alive (yet to die)
        d: dead (destroy them)
        s: spawn them
        y: yet to spawn (leave their default state)
        */

    }

    public void DestroyEnemies(GameObject enemy){
        GameObject parent = enemy.transform.parent.gameObject;
        Destroy(enemy);
        Destroy(parent);

    }
    public void SpawnEnemy(GameObject enemy, int index = -1){
        // we always spawn with telemetry becuz a new game will same with defaults and a saved one will always have changes
            enemy.SetActive(false);
            if(index!= -1){
                enemy.transform.localPosition = GetComponent<LevelManager>().levelParser.data.enemyData[index].position;
            }
            enemy.GetComponent<Spawnable>().SetAlive();
            enemy.transform.parent.gameObject.SetActive(true);
            enemy.SetActive(true);
            
    }
    public string GetEnemiesCode(){
        string s= "";
        for (int i = 0; i < Enemies.Length; i++)
        {
            if (Enemies[i] == null){
                s+="d";
            }else {
                string currState = Enemies[i].GetComponent<Spawnable>().GetStateString();
                if ( currState == "ALIVE" ){
                    s+="a";
                }else {
                    s+="y";
                }
                
            }
        }   
        return s;
    }
    public void LoadEnemiesFromCode(){
        LoadEnemies();
        try{
            for(int i =0; i < enemyCode.Length;i++){

                if (enemyCode[i] == 'a'){
                    // not that useful
                    // only set the enmy inactive
                                       
                    SpawnEnemy(Enemies[i]);
                    // set alive properties
                }
                if (enemyCode[i] == 'd'){
                    DestroyEnemies(Enemies[i]);
                }
                if(enemyCode[i] == 'y'){
                    Enemies[i].SetActive(false);
                    Enemies[i].GetComponent<Spawnable>().SetInactive();
                }

        }
        }catch (Exception e){
            print(e);
            Debug.Log("SOME PROBLEM WITH LOADING ENEMIES");
        }
    }
}
