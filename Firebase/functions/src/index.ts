import * as functions from "firebase-functions";

import * as express from "express";
import * as cors from "cors";
import {db, auth} from "./config/firebase";

const app = express();

// allow cors
app.use(cors());

const worldcollection = db.collection("worlds");
const surveycollection = db.collection("surveys");
const clickedLinkcollection = db.collection("clickedLink");

app.get("/", (req, res) => res.status(200).send("Hey there!"));

app.post("/worlds", async (req: any, res: any) => {
  let authId = null;
  try {
    let authToken = null;
    if (
      req.headers.authorization &&
        req.headers.authorization.split(" ")[0] === "Bearer"
    ) {
      authToken = req.headers.authorization.split(" ")[1];
    }
    const userInfo = await auth.verifyIdToken(authToken);
    authId =userInfo.uid;
  } catch (e:any) {
    console.log(e.message);
    return res
        .status(401)
        .send({error: "You are not authorized to make this request "+
          req.authToken});
  }
  const worldData: World = req.body;

  console.log(worldData);
  if (worldData.id || worldData.shareCode ||
    worldData.authId || !worldData.world) {
    return res.status(400).send("Invalid New World Format");
  }

  worldData.authId = authId;

  const dbWorld: DBWorld = mapWorldToDBWorld(worldData);

  try {
    const newWorld = await worldcollection.add(dbWorld);

    res.status(201).send(newWorld.id);
  } catch (err) {
    console.log(err);
    res.status(500).send();
  }
});

app.post("/surveypre", async (req: any, res: any) => {
  let authId = null;
  try {
    let authToken = null;
    if (
      req.headers.authorization &&
        req.headers.authorization.split(" ")[0] === "Bearer"
    ) {
      authToken = req.headers.authorization.split(" ")[1];
    }
    const userInfo = await auth.verifyIdToken(authToken);
    authId =userInfo.uid;
  } catch (e:any) {
    console.log(e.message);
    return res
        .status(401)
        .send({error: "You are not authorized to make this request "+
          req.authToken});
  }
  const surveyData: SurveyData = req.body;


  if (!surveyData) {
    return res.status(400).send("Invalid survey data Format");
  }

  const survey : Survey = {authId: authId, pre: true, survey: surveyData};

  try {
    const newsurveyData = await surveycollection.add(survey);

    res.status(201).send(newsurveyData.id);
  } catch (err) {
    console.log(err);
    res.status(500).send();
  }
});

app.post("/surveypost", async (req: any, res: any) => {
  let authId = null;
  try {
    let authToken = null;
    if (
      req.headers.authorization &&
        req.headers.authorization.split(" ")[0] === "Bearer"
    ) {
      authToken = req.headers.authorization.split(" ")[1];
    }
    const userInfo = await auth.verifyIdToken(authToken);
    authId =userInfo.uid;
  } catch (e:any) {
    console.log(e.message);
    return res
        .status(401)
        .send({error: "You are not authorized to make this request "+
          req.authToken});
  }
  const surveyData: SurveyData = req.body;


  if (!surveyData) {
    return res.status(400).send("Invalid survey data Format");
  }

  const survey : Survey = {authId: authId, pre: false, survey: surveyData};

  try {
    const newsurveyData = await surveycollection.add(survey);

    res.status(201).send(newsurveyData.id);
  } catch (err) {
    console.log(err);
    res.status(500).send();
  }
});

app.post("/clickedLink", async (req: any, res: any) => {
  let authId = null;
  try {
    let authToken = null;
    if (
      req.headers.authorization &&
        req.headers.authorization.split(" ")[0] === "Bearer"
    ) {
      authToken = req.headers.authorization.split(" ")[1];
    }
    const userInfo = await auth.verifyIdToken(authToken);
    authId =userInfo.uid;
  } catch (e:any) {
    console.log(e.message);
    return res
        .status(401)
        .send({error: "You are not authorized to make this request "+
          req.authToken});
  }

  const clickedLink : ClickedLink = {authId: authId};

  try {
    await clickedLinkcollection.add(clickedLink);

    res.status(201).send();
  } catch (err) {
    console.log(err);
    res.status(500).send();
  }
});

app.put("/worlds/:id", async (req: any, res: any) => {
  let authId = null;
  try {
    let authToken = null;
    if (
      req.headers.authorization &&
        req.headers.authorization.split(" ")[0] === "Bearer"
    ) {
      authToken = req.headers.authorization.split(" ")[1];
    }
    const userInfo = await auth.verifyIdToken(authToken);
    authId =userInfo.uid;
  } catch (e:any) {
    console.log(e.message);
    return res
        .status(401)
        .send({error: "You are not authorized to make this request "+
          req.authToken});
  }

  const updatedWorld: World = req.body;

  console.log("updating world " + updatedWorld.world.Name +
  "for id " + req.params.id);
  if (!updatedWorld.world) {
    return res.status(400).send("Invalid Updated World Format");
  }
  updatedWorld.authId = authId;

  try {
    console.log("Getting existing world id " + req.params.id);
    const worldDoc = worldcollection.doc(req.params.id);
    const world = await worldDoc.get();
    const worldData = world.data();

    if (!world.exists || !worldData) {
      return res.status(404).send();
    }

    console.log("Mapping " + updatedWorld.world.Name);
    const updatedDbWorld = mapWorldToDBWorld(updatedWorld);
    console.log("updated world " + updatedWorld.world.Name +
    " " + updatedWorld.id);
    await worldDoc.update({
      world: updatedDbWorld.world,
    });

    return res.status(204).send();
  } catch (err) {
    console.log("Unknown error:" + err);
    res.status(500).send();
  }
});

/**
 * Marshalling to a Firestore friendly format.
 * @param {Word} world world value
 * @return {DBWorld} returns database friendly world.
 */
function mapWorldToDBWorld(world: World): DBWorld {
  const dbWorldMap: { [ind: string]: any[] } = {};
  world.world.WorldData.map((val, ind) => {
    dbWorldMap[ind.toString()] = val;
  });

  return {
    ...world, world: {
      ...world.world, WorldData: dbWorldMap,
    },
  };
}

interface World {
    authId?: string;
    id?: string;
    shareCode?: string;
    world: {
        Name: string;
        CreationTime: string;
        PlayEndTime: string;
        WorldData: any[][]; // n x n tiles
        ResourceData: {
            Money: number;
            Population: number;
            [other: string]: any; // resource model may change
        }; // resource data
        IsTutorialFinished: boolean;
    }
}

interface DBWorld {
    authId?: string;
    id?: string;
    shareCode?: string;
    world: {
        Name: string;
        CreationTime: string;
        PlayEndTime: string;
        WorldData: {
            [ind: string]: any[] // firestore friendly format
        }; // n x n tiles
        ResourceData: {
            Money: number;
            Population: number;
            [other: string]: any; // resource model may change
        }; // resource data
        IsTutorialFinished: boolean;
    }
}
interface SurveyQuestion{
  Indexes:number[];
  Answers:string[];
}

interface SurveyData{
  Language: string;
  ResultList: SurveyQuestion[];
}
interface Survey {
  authId?: string;
  id?: string;
  pre: boolean;
  survey: SurveyData;
}
interface ClickedLink {
  authId: string;
}

exports.api = functions.https.onRequest(app);
