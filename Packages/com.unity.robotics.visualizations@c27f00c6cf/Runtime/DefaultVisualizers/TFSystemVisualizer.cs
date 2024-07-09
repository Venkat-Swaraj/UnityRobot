using System;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

namespace Unity.Robotics.Visualizations
{
    public class TFSystemVisualizer : MonoBehaviour
    {
        public float axesScale = 0.1f;
        public float lineThickness = 0.01f;
        public Color color = Color.white;

        public bool ShowTFAxesDefault { get; set; } = true;
        public bool ShowTFLinksDefault { get; set; } = true;
        public bool ShowTFNamesDefault { get; set; } = true;

        private Dictionary<string, Drawing3d> drawings = new Dictionary<string, Drawing3d>();

        private Dictionary<TFStream, bool> m_ShowAxes = new Dictionary<TFStream, bool>();
        private Dictionary<TFStream, bool> m_ShowLinks = new Dictionary<TFStream, bool>();
        private Dictionary<TFStream, bool> m_ShowNames = new Dictionary<TFStream, bool>();

        public void Start()
        {
            TFSystem.GetOrCreateInstance().AddListener(OnChanged);
            if (color.a == 0)
                color.a = 1;

            InitializeSettings();
        }

        private void InitializeSettings()
        {
            foreach (TFStream stream in TFSystem.instance.GetTransforms())
            {
                EnsureSettings(stream);
                NotifyChange(stream);
            }
        }

        void EnsureSettings(TFStream stream)
        {
            if (!m_ShowAxes.ContainsKey(stream))
            {
                m_ShowAxes.Add(stream, ShowTFAxesDefault);
                m_ShowLinks.Add(stream, ShowTFLinksDefault);
                m_ShowNames.Add(stream, ShowTFNamesDefault);
            }
        }

        public void OnChanged(TFStream stream)
        {
            Drawing3d drawing;
            if (!drawings.TryGetValue(stream.Name, out drawing))
            {
                drawing = Drawing3d.Create();
                drawing.name = stream.Name; // Set the GameObject's name to the TFStream's name
                drawings[stream.Name] = drawing;
                if (stream.Parent != null)
                {
                    OnChanged(stream.Parent);
                    Drawing3d parentStream;
                    if (drawings.TryGetValue(stream.Parent.Name, out parentStream))
                    {
                        drawing.transform.parent = parentStream.transform;
                    }
                }
            }

            TFFrame frame = stream.GetLocalTF();

            drawing.transform.localPosition = frame.translation;
            drawing.transform.localRotation = frame.rotation;
            drawing.Clear();
            EnsureSettings(stream);
            if (m_ShowAxes[stream])
                VisualizationUtils.DrawAxisVectors<FLU>(drawing, Vector3.zero.To<FLU>(), Quaternion.identity.To<FLU>(), axesScale, false);

            if (m_ShowLinks[stream])
                drawing.DrawLine(Quaternion.Inverse(frame.rotation) * -frame.translation, Vector3.zero, color, lineThickness);

            if (m_ShowNames[stream])
                drawing.DrawLabel(stream.Name, Vector3.zero, color);
        }

        private void NotifyChange(TFStream stream)
        {
            TFSystem.instance.NotifyAllChanged(stream);
        }

        // Public methods to change settings
        public void SetShowAxes(bool show)
        {
            foreach (var key in m_ShowAxes.Keys)
            {
                m_ShowAxes[key] = show;
            }
            NotifyAllChanges();
        }

        public void SetShowLinks(bool show)
        {
            foreach (var key in m_ShowLinks.Keys)
            {
                m_ShowLinks[key] = show;
            }
            NotifyAllChanges();
        }

        public void SetShowNames(bool show)
        {
            foreach (var key in m_ShowNames.Keys)
            {
                m_ShowNames[key] = show;
            }
            NotifyAllChanges();
        }

        private void NotifyAllChanges()
        {
            foreach (TFStream stream in TFSystem.instance.GetTransforms())
            {
                NotifyChange(stream);
            }
        }
    }
}
