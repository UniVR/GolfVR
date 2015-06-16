// Copyright 2015 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#import "CardboardView.h"

namespace {

enum TouchAction {
  kDown = 0,
  kUp = 1,
  kMove = 2,
  kCancel = 3,
};

}  // anonymous namespace

extern "C" {

extern bool interceptTouch(int action, NSSet* touches, CGFloat scale);

}  // extern "C"

@implementation CardboardView

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event {
  if (!interceptTouch(kDown, touches, [self contentScaleFactor])) {
    UnitySendTouchesBegin(touches, event);
  }
}

- (void)touchesEnded:(NSSet *)touches withEvent:(UIEvent *)event {
  if (!interceptTouch(kUp, touches, [self contentScaleFactor])) {
    UnitySendTouchesEnded(touches, event);
  }
}

- (void)touchesCancelled:(NSSet *)touches withEvent:(UIEvent *)event {
  if (!interceptTouch(kCancel, touches, [self contentScaleFactor])) {
    UnitySendTouchesCancelled(touches, event);
  }
}

- (void)touchesMoved:(NSSet *)touches withEvent:(UIEvent *)event {
  if (!interceptTouch(kMove, touches, [self contentScaleFactor])) {
    UnitySendTouchesMoved(touches, event);
  }
}

@end
