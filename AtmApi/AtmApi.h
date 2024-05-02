#pragma once

#include <windows.h>
#include <string>

#ifdef ATMBIZLOGIC_EXPORTS
#define ATMAPI __declspec(dllexport)
#else
#define ATMAPI __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C"
{
#endif
    typedef void (*pAtmEventCallback)(PCWSTR message);

    struct VersionData {
        USHORT major;
        USHORT minor;
        USHORT revision;
        USHORT build;
    };

    struct AppData {
        GUID appID;
        VersionData appVersionData;
    };

    constexpr size_t USERNAME_SIZE {100};
    constexpr size_t TRXID_SIZE {30};
    constexpr size_t LANGUAGEID_SIZE {6};
    constexpr size_t LOCATION_SIZE {100};


    /* We need to consider versioning */
    struct InstanceData {
        AppData clientAppData;
        AppData apiAppData;
        HANDLE atmHandle;
        wchar_t location[LOCATION_SIZE];
    };

    /* We need to consider versioning */
    struct TransactionData {
        wchar_t userName[USERNAME_SIZE];
        wchar_t trxID[TRXID_SIZE];
        wchar_t languageID[LANGUAGEID_SIZE];
        time_t transactionTime;
    };

    struct RegistrationData {
        wchar_t userName[USERNAME_SIZE];
    };

	ATMAPI HANDLE Connect(InstanceData* pInstanceData);
	ATMAPI HRESULT Disconnect(HANDLE hInstance);
	ATMAPI HRESULT StartTransaction(HANDLE hInstance, TransactionData* pTrxData);
    ATMAPI HRESULT EndTransaction(HANDLE hInstance, TransactionData *pTrxData);
	ATMAPI HRESULT RegisterCallback(HANDLE hInstance, pAtmEventCallback pCallback);
    ATMAPI HRESULT RegisterPhoneKYC(HANDLE hInstance, RegistrationData *pRegistrationData);

#ifdef __cplusplus
}
#endif
