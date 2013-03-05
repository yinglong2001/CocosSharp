/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011 Zynga Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

namespace cocos2d
{
    public class CCTransitionSlideInB : CCTransitionSlideInL
    {
        /// <summary>
        ///  initializes the scenes 
        /// </summary>
        public override void InitScenes()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            m_pInScene.Position = new CCPoint(0, -(s.Height - 0.5f));
        }

        /// <summary>
        ///  returns the action that will be performed by the incomming and outgoing scene 
        /// </summary>
        /// <returns></returns>
        public override CCActionInterval Action()
        {
            CCSize s = CCDirector.SharedDirector.WinSize;
            return CCMoveBy.Create(m_fDuration, new CCPoint(0, s.Height - 0.5f));
        }

        protected override void SceneOrder()
        {
            m_bIsInSceneOnTop = true;
        }

        public new static CCTransitionSlideInB Create(float t, CCScene scene)
        {
            var pScene = new CCTransitionSlideInB();
            pScene.InitWithDuration(t, scene);
            return pScene;
        }
    }
}