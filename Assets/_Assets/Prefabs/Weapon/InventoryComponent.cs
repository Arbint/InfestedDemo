using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    [SerializeField] Weapon[] mInitialWeaponPrefabs;
    [SerializeField] Socket mDefaultSocket;

    List<Weapon> mWeapons = new List<Weapon>();
    List<Socket> mSockets = new List<Socket>();

    int mCurrentWeaponIndex = -1;

    private void Awake()
    {
        mSockets.AddRange(GetComponentsInChildren<Socket>());

        foreach(Weapon weaponPrefab in mInitialWeaponPrefabs)
        {
            Weapon newWeapon = Instantiate(weaponPrefab);
            newWeapon.transform.SetParent(GetSocketForWeapon(newWeapon).transform, false);
            newWeapon.Init(gameObject);
            mWeapons.Add(newWeapon);
        }

        SwitchToNextWeapon();
    }
    public void SwitchToNextWeapon()
    {
        int nextWeaponIndex = (mCurrentWeaponIndex + 1) % mWeapons.Count;
        SwitchToWeaponByIndex(nextWeaponIndex); 
    }

    void SwitchToWeaponByIndex(int index)
    {
        if(index < 0 && index >= mWeapons.Count)
        {
            return;
        }

        if(mCurrentWeaponIndex >= 0 && mCurrentWeaponIndex < mWeapons.Count)
        {
            mWeapons[mCurrentWeaponIndex].UnEquip();
        }

        mWeapons[index].Equip();
        mCurrentWeaponIndex = index;
    }

    Socket GetSocketForWeapon(Weapon weapon)
    {
        Socket weaponSocket = mSockets.Find((socket) => { return socket.IsFor(weapon.AttachSocketName); });
        if(!weaponSocket)
        {
            weaponSocket = mDefaultSocket; 
        }

        return weaponSocket;
    }

    internal void FireCurrentWeapon()
    {
        mWeapons[mCurrentWeaponIndex].Fire();
    }
}
