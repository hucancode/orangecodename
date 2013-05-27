﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Orange.GameData.Materials;
using System.IO;
using System.Xml;

namespace Orange.GameProcessing.Entities
{
    public class Brick : Character
    {
        public bool Vulnerable;
        private int[] idleFrame;
        private int[] explodeFrame;
        public Brick(Vector2 gridPos, string name)
        {
            gridPosition = gridPos;
            mapPosition = gridPosition * 42;
            state = 0;
            {
                string data = File.ReadAllText("Content/Brick/" + name + ".xml");
                XmlDocument document = new XmlDocument();
                document.LoadXml(data);
                XmlElement mobTAG = (XmlElement)document.FirstChild.NextSibling;
                XmlElement textureTAG = (XmlElement)mobTAG.FirstChild;
                XmlElement attributeTAG = (XmlElement)textureTAG.NextSibling;
                XmlElement animationTAG = (XmlElement)attributeTAG.NextSibling;
                XmlElement idleTAG = (XmlElement)animationTAG.FirstChild;
                XmlElement moveTAG = (XmlElement)idleTAG.NextSibling;
                XmlElement birthTAG = (XmlElement)moveTAG.NextSibling;
                XmlElement dieTAG = (XmlElement)birthTAG.NextSibling;

                string texture = textureTAG.GetAttribute("file");
                int dx = int.Parse(textureTAG.GetAttribute("divide_x"));
                int dy = int.Parse(textureTAG.GetAttribute("divide_y"));
                int ox = int.Parse(textureTAG.GetAttribute("offset_x"));
                int oy = int.Parse(textureTAG.GetAttribute("offset_y"));
                animation = new Animation("Brick/" + texture,
                    mapPosition, (int)gridPosition.Y, dx, dy);
                animation.original = new Vector2(ox, oy);
                animation.delay = 60;

                int idleStart = int.Parse(idleTAG.GetAttribute("begin"));
                int idleEnd = int.Parse(idleTAG.GetAttribute("end"));
                idleFrame = new int[2] { idleStart, idleEnd };
                int explodeStart = int.Parse(dieTAG.GetAttribute("begin"));
                int explodeEnd = int.Parse(dieTAG.GetAttribute("end"));
                explodeFrame = new int[2] { explodeStart, explodeEnd };
            }
            animation.newAnimation(idleFrame[0], idleFrame[1]);
        }
        public override void Update()
        {
            base.Update();
            if (animation.isStop) state = 1;
        }
        public void Kill()
        {
            animation.isLoop = false;
            animation.newAnimation(explodeFrame[0], explodeFrame[1]);
        }
    }
}
