using System;
using CustomTimer;
using UnityEngine;

namespace Unit.ProfessionsCore.Processors
{
    public class ResourceExtractionProcessor
    {
        private readonly Timer _extractionTimer;
        private readonly GameObject _resourceSkin;
        private readonly ResourceRepository _resourceRepository;
        
        public int ExtractionCapacity  { get; private set; }
        public ResourceID ExtractedResourceID { get; private set; }
        public bool GotResource { get; private set; } = false;
        public bool Extraction { get; private set; } = false;
        
        public event Action OnResourceExtracted;
        public event Action OnStorageResources;
        
        public ResourceExtractionProcessor(int gatheringCapacity, float extractionTime, ResourceRepository resourceRepository, GameObject resourceSkin)
        {
            ExtractionCapacity = gatheringCapacity;
            _extractionTimer = new Timer(extractionTime, 0, true);
            _extractionTimer.OnTimerEnd += ExtractResource;
            _resourceRepository = resourceRepository;
            _resourceSkin = resourceSkin;
            HideResource();
        }
        
        public void HandleUpdate(float time) => _extractionTimer.Tick(time);
        
        /// <summary>
        /// Start resource extraction timer
        /// </summary>
        public void StartExtraction(ResourceID resourceID)
        {
            if(Extraction) return;
            
            Extraction = true;
            ExtractedResourceID = resourceID;
            _extractionTimer.Reset();
        }

        /// <summary>
        /// Abort resource extraction timer
        /// </summary>
        public void AbortExtraction()
        {
            if(!Extraction) return;
            
            Extraction = false;
            _extractionTimer.Reset(true);
        }
        
        /// <summary>
        /// Give order put resource in storage
        /// </summary>
        public void StorageResources()
        {
            if(!GotResource) return;
            
            _resourceRepository.ChangeValue(ExtractedResourceID, ExtractionCapacity);
            GotResource = false;
            HideResource();
            OnStorageResources?.Invoke();
        }
        
        private void ShowResource() => _resourceSkin.SetActive(true);
        private void HideResource() => _resourceSkin.SetActive(false);
        
        private void ExtractResource()
        {
            GotResource = true;
            Extraction = false;
            ShowResource();
            OnResourceExtracted?.Invoke();
        }
    }
}