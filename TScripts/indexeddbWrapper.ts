let dotnetReference: any;
let database: IDBDatabase;

export function initializeDatabase(reference: any, dbName: string) {
  dotnetReference = reference;

  const openRequest = window.indexedDB.open(dbName, 4);

  openRequest.onsuccess = async () => {
    console.log("initialized database " + dbName);
    database = openRequest.result;
    await dotnetReference.invokeMethodAsync("DatabaseInitialized");
  };

  openRequest.onupgradeneeded = () => {
    database = openRequest.result;
    database.createObjectStore("pairs", {
      keyPath: "id",
      autoIncrement: true,
    });
  };

  openRequest.onerror = (event) => {
    console.error(`cannot open database with name ${dbName}: ${event}`);
  };
}

export async function putPair(key: string, value: string, secret: string) {
  const store = database.transaction("pairs", "readwrite").objectStore("pairs");
  const request = store.put({ key, value });
  request.onsuccess = async () => {
    await dotnetReference.invokeMethodAsync("PairSaved");
  };
}

export async function deletePair(id: number) {
  const store = database.transaction("pairs", "readwrite").objectStore("pairs");
  const request = store.delete(id);
  request.onsuccess = async () => {
    await dotnetReference.invokeMethodAsync("PairDeleted", id);
  };
}

export function getAllPairs() {
  const store = database.transaction("pairs", "readonly").objectStore("pairs");
  const request = store.getAll();
  request.onsuccess = async () => {
    await dotnetReference.invokeMethodAsync("UpdatePairs", request.result);
  };
}
